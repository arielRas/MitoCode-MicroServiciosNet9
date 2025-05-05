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
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentRequestedEvent> context)
        {
            var message = context.Message;

            try
            {
                _logger.LogInformation($"[SAGA] - Received {nameof(PaymentRequestedEvent)} - CorrelationId: {message.CorrelationId}");

                if (!await _orderRepository.ExistsAsync(message.CorrelationId))
                    throw new NonExistentResourceException(message.CorrelationId, $"The order with id {message.CorrelationId} to which the payment refers does not exist");

                var payment = new Payment
                {
                    OrderId = message.CorrelationId,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdate = DateTime.UtcNow,
                    Status = PaymentStatus.Pending,
                };

                await _paymentRepository.CreateAsync(payment);

                _logger.LogInformation($"[SAGA] - A new Payment has been created - OrderId: {payment.OrderId}");
            }
            catch (AsynchronousMessagingException ex)
            {
                var paymentFailed = new PaymentFailedEvent 
                {
                    CorrelationId = message.CorrelationId,
                    Reason = ex.Message
                };

                await context.Publish(paymentFailed, ctx =>
                {
                    ctx.CorrelationId = message.CorrelationId;
                });

                _logger.LogInformation($"[SAGA] - Send {nameof(PaymentFailedEvent)} - CorrelationId: {message.CorrelationId} - Reason: {ex.Message}");

            }
            catch (Exception ex)
            {
                _logger.LogError($"[SAGA] - An unexpected error has occurred - {ex.Message}");
            }
        }
    }
}
