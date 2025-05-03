using FastBuy.Payments.Api.DTOs;
using FastBuy.Payments.Api.Exceptions;
using FastBuy.Payments.Api.Mappers;
using FastBuy.Payments.Api.Persistence.Repository.Abstractions;
using FastBuy.Payments.Api.Services.Abstractions;

namespace FastBuy.Payments.Api.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;

        public PaymentService(IPaymentRepository paymentRepository, IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
        }


        public async Task CreateAsync(PaymentRequestDto paymentDto)
        {
            var order = await _orderRepository.GetByIdAsync(paymentDto.OrderId)
                ?? throw new KeyNotFoundException($"The order with id {paymentDto.OrderId} does not exist");

            if (order.Amount != paymentDto.Amount)
                throw new BusinessException("The amount entered is different from the amount to be paid");

            var payment = paymentDto.ToEntity();

            await _paymentRepository.CreateAsync(payment);
        }
    }
}
