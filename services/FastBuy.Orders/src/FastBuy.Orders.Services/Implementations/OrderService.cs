using FastBuy.Orders.Contracts.DTOs;
using FastBuy.Orders.Repository.Repositories.Abstractions;
using FastBuy.Orders.Services.Abstractions;
using FastBuy.Orders.Services.Mappers;
using MassTransit;

namespace FastBuy.Orders.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPublishEndpoint _publisher;

        public OrderService(IOrderRepository orderRepository, IPublishEndpoint publisher)
        {
            _orderRepository = orderRepository;
            _publisher = publisher;
        }

        public async Task<OrderResponseDto> GetByIdAsync(Guid id)
        {
            var order = (await _orderRepository.GetOrderWithRelationshipsAsync(id)).ToDto();

            order.State = await _orderRepository.GetOrderStateAsync(id) ?? "Order status is not available";

            return order;

        }

        public async Task CreateAsync(OrderRequestDto orderDto)
        {
            var newOrder = orderDto.ToEntity();

            await _orderRepository.CreateAsync(newOrder);

            var orderAmount = await _orderRepository.GetOrderAmountAsync(newOrder.OrderId);

            var orderCreateEvent = newOrder.ToOrderCreateEvent(orderAmount);

            await _publisher.Publish(orderCreateEvent, ctx =>
            {
                ctx.CorrelationId = orderCreateEvent.CorrelationId;
            });
        }        
    }
}
