using FastBuy.Payments.Api.DTOs;

namespace FastBuy.Payments.Api.Services.Abstractions
{
    public interface IOrderService
    {
        Task<OrderResponseDto> GetOrderAsync(Guid orderId);
    }
}
