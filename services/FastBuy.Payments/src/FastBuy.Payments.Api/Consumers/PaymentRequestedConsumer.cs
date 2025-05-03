using FastBuy.Payments.Api.Entities;
using FastBuy.Payments.Api.Persistence.Repository.Abstractions;
using FastBuy.Shared.Events.Exceptions;
using FastBuy.Shared.Events.Saga.Payments;
using MassTransit;

namespace FastBuy.Payments.Api.Consumers
{
    public class PaymentRequestedConsumer : IConsumer<PaymentRequestedEvent>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<PaymentRequestedConsumer> _logger;

        public PaymentRequestedConsumer(IPaymentRepository paymentRepository, IOrderRepository orderRepository, ILogger<PaymentRequestedConsumer> logger)
        {
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
            _logger = logger;
        }


        public async Task Consume(ConsumeContext<PaymentRequestedEvent> context)
        {
            var message = context.Message;

            try
            {
                _logger.LogInformation($"[SAGA] - Recived {nameof(PaymentRequestedEvent)} - CorrelationId: {message.CorrelationId}");

                if (await _orderRepository.ExistsAsync(message.OrderId))
                    throw new ExistingResourceException(message.CorrelationId, $"The order with id {message.OrderId} already exists and is involved in another operation.");

                var order = new Order
                {
                    OrderId = message.OrderId,
                    CreatedAt = DateTime.UtcNow,
                    Amount = message.Amount
                };

                await _orderRepository.CreateAsync(order);

                _logger.LogInformation($"[SAGA] - A new order has been created - OrderId: {order.OrderId}");
            }
            catch(AsynchronousMessagingException ex)
            {
                _logger.LogInformation($"[SAGA] - The order could not be created - Reason: {ex.Message}");

                var paymentFailed = new PaymentFailedEvent
                {
                    CorrelationId = message.CorrelationId,
                    Reason = ex.Message
                };

                await context.Publish(paymentFailed, ctx =>
                {
                    ctx.CorrelationId = message.CorrelationId;
                });

                _logger.LogInformation($"[SAGA] - Send {nameof(PaymentFailedEvent)} - CorrelationId: {message.CorrelationId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[SAGA] - An unexpected error has occurred - {ex.Message}");
            }
        }
    }
}
