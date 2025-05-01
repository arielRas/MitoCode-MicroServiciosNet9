using FastBuy.Orders.Repository.Saga;
using FastBuy.Shared.Events.Saga.Orders;
using FastBuy.Shared.Events.Saga.Stocks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FastBuy.Orders.Services.StateMachines;

public class OrderStateMachine : MassTransitStateMachine<OrderState>
{
    private readonly ILogger<OrderStateMachine> _logger;

    public State Created { get; }
    public State AwaitingStocksDecrease  { get; }
    public State AwaitingStocksIncrease { get; }
    public State AwaitingPayment { get; }
    public State Rejected { get; }
    public State Completed { get; }


    public Event<OrderCreatedEvent> OrderCreated { get; }
    public Event<StockDecreasedEvent> StockDecreased { get; }
    public Event<StockIncreasedEvent> StockIncreased { get; }
    public Event<StockDecreaseFailedEvent> StockFailedDecrease { get; }
    public Event<StockIncreaseFailedEvent> StockFailedIncrease { get; }


    public OrderStateMachine(ILogger<OrderStateMachine> logger)
    {
        _logger = logger;
        InstanceState(state => state.CurrentState);
        ConfigureEvents();
        ConfigureInitialState();
        ConfigureAwaitingStoksDecrease();
        ConfigureAwaitingStockIncrease();
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
            When(OrderCreated).Then(context =>
            {
                context.Saga.CreatedAt = DateTime.UtcNow;

                context.Saga.LastUpdate = DateTime.UtcNow;

                context.Saga.CurrentState = nameof(AwaitingStocksDecrease);

                _logger.LogInformation($"[SAGA] Order create - CorrelationId {context.Saga.CorrelationId}");
            })
            .TransitionTo(AwaitingStocksDecrease)
        );
    }

    private void ConfigureAwaitingStoksDecrease()
    {
        During(AwaitingStocksDecrease,
             When(StockDecreased).Then(context =>
             {
                 context.Saga.LastUpdate = DateTime.UtcNow;

                 context.Saga.CurrentState = nameof(Completed);

                 _logger.LogInformation($"[SAGA] Decreased stock - CorrelationId {context.Saga.CorrelationId}");
             })  
             .TransitionTo(Completed),


             When(StockFailedDecrease).ThenAsync(async context =>
             {
                 _logger.LogInformation($"[SAGA] Failed decrease stock - CorrelationId {context.Saga.CorrelationId} - Reason: {context.Message.Reason ?? "Unknown Reason"}");

                 var message = context.Message;

                 context.Saga.LastUpdate = DateTime.UtcNow;

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

                     await context.TransitionToState(AwaitingStocksIncrease);

                     context.Saga.CurrentState = nameof(AwaitingStocksIncrease);

                     _logger.LogInformation($"[SAGA] Generate {nameof(StockIncreaseRequestedEvent)} - CorrelationId {stockIncreasedRequested.CorrelationId}");
                 }
                 else
                 {
                     context.Saga.CurrentState = nameof(Rejected);

                     await context.TransitionToState(Rejected); 
                 }             
             })
        );
    }

    private void ConfigureAwaitingStockIncrease()
    {
        During(AwaitingStocksIncrease,
            When(StockIncreased).Then(context =>
            {
                context.Saga.LastUpdate = DateTime.UtcNow;

                context.Saga.CurrentState = nameof(Rejected);

                _logger.LogInformation($"[SAGA] - Received {nameof(StockIncreasedEvent)} - Order change to {nameof(Rejected)} state - CorrelationId {context.Saga.CorrelationId}");
            })
            .TransitionTo(Rejected)
        );
    }
}
