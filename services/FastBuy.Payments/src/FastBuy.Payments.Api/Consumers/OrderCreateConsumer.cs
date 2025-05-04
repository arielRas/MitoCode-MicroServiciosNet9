using FastBuy.Payments.Api.Entities;
using FastBuy.Payments.Api.Persistence.Repository.Abstractions;
using FastBuy.Shared.Events.Exceptions;
using FastBuy.Shared.Events.Saga.Orders;
using MassTransit;

namespace FastBuy.Payments.Api.Consumers
{
    public class OrderCreateConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly IOrderRepository _repository;
        private readonly ILogger<OrderCreateConsumer> _logger;

        public OrderCreateConsumer(IOrderRepository repository, ILogger<OrderCreateConsumer> logger)
        {
            _repository = repository;
            _logger = logger;
        }


        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var message = context.Message;

            try
            {
                _logger.LogInformation($"[SAGA] - Received {nameof(OrderCreatedEvent)} - CorrelationId: {message.CorrelationId}");

                if (await _repository.ExistsAsync(message.OrderId))
                    throw new ExistingResourceException(message.CorrelationId, $"The order with id {message.OrderId} already exists and is involved in another operation.");

                var order = new Order
                {
                    OrderId = message.OrderId,
                    CreatedAt = DateTime.UtcNow,                    
                    Amount = message.Amount
                };

                await _repository.CreateAsync(order);

                _logger.LogInformation($"[SAGA] - A new order has been created - OrderId: {order.OrderId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[SAGA] - An unexpected error has occurred - {ex.Message}");
            }
        }
    }
}
