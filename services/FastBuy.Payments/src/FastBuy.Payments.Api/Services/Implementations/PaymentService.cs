using FastBuy.Payments.Api.DTOs;
using FastBuy.Payments.Api.Exceptions;
using FastBuy.Payments.Api.Mappers;
using FastBuy.Payments.Api.Persistence.Repository.Abstractions;
using FastBuy.Payments.Api.Services.Abstractions;
using FastBuy.Shared.Events.Saga.Payments;
using MassTransit;

namespace FastBuy.Payments.Api.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IPublishEndpoint _publisher;
        private readonly ILogger _logger;

        public PaymentService(IPaymentRepository paymentRepository, IOrderRepository orderRepository, IPublishEndpoint publisher, ILogger logger)
        {
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
            _publisher = publisher;
            _logger = logger;
        }

        public async Task<PaymentResponseDto> GetByIdAsync(Guid id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Payment with id {id} does not exist");

            return payment.ToDto();            
        }


        public async Task CreateAsync(PaymentRequestDto paymentDto)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(paymentDto.OrderId)
                    ?? throw new KeyNotFoundException($"The order with id {paymentDto.OrderId} does not exist");

                if (order.Amount != paymentDto.Amount)
                    throw new BusinessException("The amount entered is different from the amount to be paid");

                var payment = paymentDto.ToEntity();

                await _paymentRepository.CreateAsync(payment);

                await SendSuccessPaymentEvent(payment.OrderId);
            }
            catch(BusinessException ex)
            {
                await SendFailedPaymentEvent(paymentDto.OrderId, ex.Message ?? string.Empty);

                throw;
            }
        }


        private async Task SendSuccessPaymentEvent(Guid correlationId)
        {
            var paymentSuccess = new PaymentSuccessEvent
            {
                CorrelationId = correlationId
            };

            await _publisher.Publish(paymentSuccess, ctx =>
            {
                ctx.CorrelationId = correlationId;
            });

            _logger.LogInformation($"[SAGA] - Send {nameof(PaymentSuccessEvent)} - CorrelationId: {correlationId}");
        }


        private async Task SendFailedPaymentEvent(Guid correlationId, string reason)
        {
            var paymentFailed = new PaymentFailedEvent
            {
                CorrelationId = correlationId,
                Reason = reason
            };

            await _publisher.Publish(paymentFailed, ctx =>
            {
                ctx.CorrelationId = correlationId;
            });

            _logger.LogInformation($"[SAGA] - Send {nameof(PaymentFailedEvent)} - CorrelationId: {correlationId}");
        }
    }
}
