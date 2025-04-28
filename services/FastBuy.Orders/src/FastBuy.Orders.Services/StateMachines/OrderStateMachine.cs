using FastBuy.Orders.Contracts.Events;
using FastBuy.Orders.Repository.Saga;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FastBuy.Orders.Services.StateMachines
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        private readonly ILogger<OrderStateMachine> _logger;

        public State Created { get; }
        public State AwaitingStocksDecrease  { get; }
        public State AwaitingPayment { get; }
        public State Rejected { get; }
        public State Completed { get; }


        public Event<OrderCreatedEvent> OrderCreated { get; }
        public Event<StockDecreasedEvent> StockDecreased { get; }
        public Event<StockFailedDecreaseEvent> StockFailedDecrease { get; }


        public OrderStateMachine(ILogger<OrderStateMachine> logger)
        {
            _logger = logger;
            InstanceState(state => state.CurrentState);
            ConfigureEvents();
            ConfigureInitialState();
            ConfigureAwaitingStoksDecrease();
        }

        private void ConfigureEvents()
        {
            Event(() => OrderCreated, x =>
            {
                x.CorrelateById(context => context.Message.CorrelationId);
            }); 
            
            Event(() => StockFailedDecrease, x =>
            {
                x.CorrelateById(context => context.Message.CorrelationId);
            });

            Event(() => StockDecreased, x =>
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
                    context.Saga.CurrentState = nameof(OrderCreated);
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
                     _logger.LogInformation($"[SAGA] Decreased stock - CorrelationId {context.Saga.CorrelationId}");
                 })  
                 .TransitionTo(Completed),

                 When(StockFailedDecrease).Then(context =>
                 {
                     context.Saga.LastUpdate = DateTime.UtcNow;
                     _logger.LogInformation($"[SAGA] Failed decrease stock - CorrelationId {context.Saga.CorrelationId} - Reason: {context.Message.Reason}");
                 })
                 .TransitionTo(Rejected)
            );
        }
    }
}
