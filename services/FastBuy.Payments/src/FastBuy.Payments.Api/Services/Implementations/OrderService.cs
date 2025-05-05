using FastBuy.Payments.Api.DTOs;
using FastBuy.Payments.Api.Mappers;
using FastBuy.Payments.Api.Persistence.Repository.Abstractions;
using FastBuy.Payments.Api.Services.Abstractions;

namespace FastBuy.Payments.Api.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderResponseDto> GetOrderAsync(Guid orderId)
        {
            var order = await _orderRepository.GetOrderWithPaymentAsync(orderId);

            return order.ToDto();
        }
    }
}
