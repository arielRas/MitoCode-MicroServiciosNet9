using FastBuy.Orders.Repository.Database.Entities;
using FastBuy.Orders.Repository.Repositories.Abstractions;
using FastBuy.Orders.Services.Mappers;
using FastBuy.Shared.Events.Saga.Orders;
using FastBuy.Shared.Events.Saga.Payments;
using FastBuy.Shared.Events.Saga.Stocks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FastBuy.Orders.Services.StateMachines;

public class OrderStateMachine : MassTransitStateMachine<OrderState>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<OrderStateMachine> _logger;

    public State Created { get; }
    public State AwaitingStocksDecrease { get; }
    public State AwaitingStocksIncrease { get; }
    public State AwaitingPayment { get; }
    public State Rejected { get; }
    public State Completed { get; }


    public Event<OrderCreatedEvent> OrderCreated { get; }
    public Event<StockDecreasedEvent> StockDecreased { get; }
    public Event<StockIncreasedEvent> StockIncreased { get; }
    public Event<StockDecreaseFailedEvent> StockFailedDecrease { get; }
    public Event<StockIncreaseFailedEvent> StockFailedIncrease { get; }
    public Event<PaymentSuccessEvent> PaymentSucces {  get; }
    public Event<PaymentFailedEvent> PaymentFailed { get; }

    public OrderStateMachine(IOrderRepository orderRepository, ILogger<OrderStateMachine> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
        InstanceState(state => state.CurrentState);
        ConfigureEvents();
        ConfigureInitialState();
        ConfigureAwaitingStoksDecrease();
        ConfigureAwaitingStockIncrease();
        ConfigureAwaitingPayment();
    }

    private void ConfigureEvents()
    {
        Event(() => OrderCreated, x =>
        {
            x.CorrelateById(context => context.Message.CorrelationId);
        });

        Event(() => StockDecreased, x =>
        {
            x.CorrelateById(context => context.Message.CorrelationId);
        });

        Event(() => StockFailedDecrease, x =>
        {
            x.CorrelateById(context => context.Message.CorrelationId);
        });

        Event(() => StockFailedIncrease, x =>
        {
            x.CorrelateById(context => context.Message.CorrelationId);
        });
    }

    private void ConfigureInitialState()
    {
        Initially(
            When(OrderCreated).ThenAsync(async context =>
            {  
                _logger.LogInformation($"[SAGA] Received {nameof(OrderCreatedEvent)}  - CorrelationId {context.Saga.CorrelationId}");

                context.Saga.LastUpdate = DateTime.UtcNow;

                var stockDecreaseEvent = new StockDecreaseRequestedEvent
                {
                    CorrelationId = context.Message.CorrelationId,
                    Items = context.Message.Items
                };

                await context.Publish(stockDecreaseEvent, ctx =>
                {
                    ctx.CorrelationId = stockDecreaseEvent.CorrelationId;
                });

                _logger.LogInformation($"[SAGA] Send {nameof(StockDecreaseRequestedEvent)} - CorrelationId {context.Saga.CorrelationId}");

                _logger.LogInformation($"[SAGA] The order has changed from {nameof(Created)} to {nameof(AwaitingStocksDecrease)} status");
            })
            .TransitionTo(AwaitingStocksDecrease)
        );
    }

    private void ConfigureAwaitingStoksDecrease()
    {
        During(AwaitingStocksDecrease,
             When(StockDecreased).Then(async context =>
             {
                 _logger.LogInformation($"[SAGA] Received {nameof(StockDecreasedEvent)} - CorrelationId {context.Message.CorrelationId}");

                 context.Saga.LastUpdate = DateTime.UtcNow;
                 
                 _logger.LogInformation($"[SAGA] The order has changed from {nameof(AwaitingStocksDecrease)} to {nameof(AwaitingPayment)} status");

                 var paymentRequested = new PaymentRequestedEvent { CorrelationId = context.Message.CorrelationId};

                 await context.Publish(paymentRequested, ctx =>
                 {
                     ctx.CorrelationId = context.Message.CorrelationId;
                 });

                 _logger.LogInformation($"[SAGA] Send {nameof(PaymentRequestedEvent)} - CorrelationId {paymentRequested.CorrelationId}");
             })
             .TransitionTo(AwaitingPayment),


             When(StockFailedDecrease).ThenAsync(async context =>
             {
                 _logger.LogInformation($"[SAGA] Received {nameof(StockDecreaseFailedEvent)} - CorrelationId {context.Saga.CorrelationId} - Reason failure: {context.Message.Reason ?? "Unknown Reason"}");

                 var message = context.Message;

                 context.Saga.LastUpdate = DateTime.UtcNow;

                 context.Saga.ReasonRejection = string.IsNullOrEmpty(context.Saga.ReasonRejection)
                                                ? context.Message.Reason
                                                : context.Saga.ReasonRejection += $" - {context.Message.Reason}";

                 if (message.DiscountedItems is not null && message.DiscountedItems.Any())
                 {
                     var stockIncreasedRequested = new StockIncreaseRequestedEvent
                     {
                         CorrelationId = message.CorrelationId,
                         Items = message.DiscountedItems
                     };

                     await context.Publish(stockIncreasedRequested, ctx =>
                     {
                         ctx.CorrelationId = message.CorrelationId;
                     });

                     _logger.LogInformation($"[SAGA] Send {nameof(StockIncreaseRequestedEvent)} - CorrelationId {stockIncreasedRequested.CorrelationId}");

                     await context.TransitionToState(AwaitingStocksIncrease);

                     _logger.LogInformation($"[SAGA] The order has changed to the state: {nameof(AwaitingStocksIncrease)}");                         
                 }
                 else
                 {
                     context.Saga.CurrentState = nameof(Rejected);

                     await context.TransitionToState(Rejected);

                     _logger.LogInformation($"[SAGA] The order has changed to the state: {nameof(Rejected)}");
                 }
             })
        );
    }

    private void ConfigureAwaitingPayment()
    {
        During(AwaitingPayment,
            When(PaymentSucces).Then(context => 
            {
                _logger.LogInformation($"[SAGA] Received {nameof(PaymentSuccessEvent)} - CorrelationId {context.Message.CorrelationId}");

                context.Saga.LastUpdate = DateTime.UtcNow;

                _logger.LogInformation($"[SAGA] The order has changed from {nameof(AwaitingPayment)} to {nameof(Completed)} status");
            })
            .TransitionTo(Completed),

            When(PaymentFailed).ThenAsync(async context =>
            {
                _logger.LogInformation($"[SAGA] Received {nameof(PaymentFailedEvent)} - CorrelationId {context.Message.CorrelationId}");

                var stockIncrese = new StockIncreaseRequestedEvent
                {
                    CorrelationId = context.Message.CorrelationId,
                    Items = (await _orderRepository.GetOrderItemAsync(context.Message.CorrelationId)).Select(oi => oi.ToEvent())
                };

                await context.Publish(stockIncrese, ctx =>
                {
                    ctx.CorrelationId = stockIncrese.CorrelationId;
                });

                _logger.LogInformation($"[SAGA] Send {nameof(StockIncreaseRequestedEvent)} - CorrelationId {context.Message.CorrelationId}");

                context.Saga.ReasonRejection = string.IsNullOrEmpty(context.Saga.ReasonRejection)
                                               ? context.Message.Reason
                                               : context.Saga.ReasonRejection += $" - {context.Message.Reason}";

                context.Saga.LastUpdate = DateTime.UtcNow;

                _logger.LogInformation($"[SAGA] The order has changed from {nameof(AwaitingPayment)} to {nameof(AwaitingStocksIncrease)} status");
            })
            .TransitionTo(AwaitingStocksIncrease)
        );
        
    }

    private void ConfigureAwaitingStockIncrease()
    {
        During(AwaitingStocksIncrease,
            When(StockIncreased).Then(context =>
            {
                _logger.LogInformation($"[SAGA] - Received {nameof(StockIncreasedEvent)} - CorrelationId {context.Saga.CorrelationId}");

                context.Saga.LastUpdate = DateTime.UtcNow;

                _logger.LogInformation($"[SAGA] The order has changed from {nameof(AwaitingStocksIncrease)} to {nameof(Rejected)} status");
            })
            .TransitionTo(Rejected)
        );
    }
}
