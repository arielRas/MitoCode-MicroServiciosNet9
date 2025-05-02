using FastBuy.Orders.Contracts.DTOs;
using FastBuy.Orders.Repository.Repositories.Abstractions;
using FastBuy.Orders.Services.Abstractions;
using FastBuy.Orders.Services.Mappers;
using FastBuy.Shared.Events.Saga.Orders;
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

        public async Task CreateAsync(OrderRequestDto orderDto)
        {            
            var newId = Guid.NewGuid();

            var newOrder = orderDto.ToEntity(newId);

            await _orderRepository.CreateAsync(newOrder);

            var orderCreateEvent = newOrder.ToOrderCreateEvent(newId, 100);

            await _publisher.Publish(orderCreateEvent, ctx =>
            {
                ctx.CorrelationId = newOrder.Id;
            });
        }
    }
}
