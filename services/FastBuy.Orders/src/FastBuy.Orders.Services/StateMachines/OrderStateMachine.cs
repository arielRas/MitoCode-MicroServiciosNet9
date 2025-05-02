using FastBuy.Orders.Repository.Database.Entities;
using FastBuy.Shared.Events.Saga.Orders;
using FastBuy.Shared.Events.Saga.Stocks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FastBuy.Orders.Services.StateMachines
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
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
                 When(StockDecreased).Then(context =>
                 {
                     _logger.LogInformation($"[SAGA] Received {nameof(StockDecreasedEvent)} - CorrelationId {context.Saga.CorrelationId}");

                     context.Saga.LastUpdate = DateTime.UtcNow;
                     
                     _logger.LogInformation($"[SAGA] The order has changed from {nameof(AwaitingStocksDecrease)} to {nameof(Completed)} status");
                 })
                 .TransitionTo(Completed),


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
}
