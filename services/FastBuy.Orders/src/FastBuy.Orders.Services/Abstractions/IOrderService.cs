using FastBuy.Orders.Contracts.DTOs;

namespace FastBuy.Orders.Services.Abstractions
{
    public interface IOrderService
    {
        Task CreateAsync(OrderRequestDto orderDto);
        Task<OrderResponseDto> GetByIdAsync(Guid id);
    }
}
