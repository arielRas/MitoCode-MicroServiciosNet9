using FastBuy.Payments.Api.Entities;
using FastBuy.Payments.Api.Persistence.UnitOfWork;
using FastBuy.Shared.Events.Exceptions;
using FastBuy.Shared.Events.Saga.Payments;
using MassTransit;

namespace FastBuy.Payments.Api.Consumers
{
    public class PaymentRequestedConsumer : IConsumer<PaymentRequestedEvent>
    {
        private readonly IPaymentUnitOfWork _unitOfWork;
        private readonly ILogger<PaymentRequestedConsumer> _logger;

        public PaymentRequestedConsumer(IPaymentUnitOfWork unitOfWork, ILogger<PaymentRequestedConsumer> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }


        public async Task Consume(ConsumeContext<PaymentRequestedEvent> context)
        {
            var message = context.Message;

            try
            {
                //_logger.LogInformation($"[SAGA] - Recived {nameof(PaymentRequestedEvent)} - CorrelationId: {message.CorrelationId}");

                //if (await _unitOfWork.OrderRepository.ExistsAsync(message.OrderId))
                //    throw new ExistingResourceException(message.CorrelationId, $"The order with id {message.OrderId} already exists and is involved in another operation.");

                //var order = new Order
                //{
                //    OrderId = message.OrderId,
                //    CreatedAt = DateTime.UtcNow,
                //    Amount = message.Amount
                //};

                //var payment = new Payment
                //{
                //    OrderId = message.OrderId,
                //    CreatedAt = DateTime.UtcNow,
                //    Status = PaymentStates.Pending
                //};

                //await _unitOfWork.BeginTransactionAsync();

                //await _unitOfWork.OrderRepository.CreateAsync(order);

                //await _unitOfWork.PaymentRepository.CreateAsync(payment);

                //await _unitOfWork.CommitTransactionAsync();

                //_logger.LogInformation($"[SAGA] - A new order has been created - OrderId: {order.OrderId}");
            }
            catch(AsynchronousMessagingException ex)
            {
                //_logger.LogInformation($"[SAGA] - The order could not be created - Reason: {ex.Message}");

                //var paymentFailed = new PaymentFailedEvent
                //{
                //    CorrelationId = message.CorrelationId,
                //    Reason = ex.Message
                //};

                //await context.Publish(paymentFailed, ctx =>
                //{
                //    ctx.CorrelationId = message.CorrelationId;
                //});

                //_logger.LogInformation($"[SAGA] - Send {nameof(PaymentFailedEvent)} - CorrelationId: {message.CorrelationId}");
            }
            catch (Exception ex)
            {
                //await _unitOfWork.RollbackTransactionAsync();

                //var paymentFailed = new PaymentFailedEvent
                //{
                //    CorrelationId = message.CorrelationId,
                //    Reason = ex.Message
                //};

                //await context.Publish(paymentFailed, ctx =>
                //{
                //    ctx.CorrelationId = message.CorrelationId;
                //});

                //_logger.LogError($"[SAGA] - An unexpected error has occurred - {ex.Message}");
            }
        }
    }
}
