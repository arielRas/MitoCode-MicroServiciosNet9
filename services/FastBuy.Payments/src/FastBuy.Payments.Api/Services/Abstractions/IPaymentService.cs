using FastBuy.Payments.Api.DTOs;

namespace FastBuy.Payments.Api.Services.Abstractions
{
    public interface IPaymentService
    {
        Task CreateAsync(PaymentRequestDto paymentDto);
        Task<PaymentResponseDto> GetByIdAsync(Guid id);
    }
}
