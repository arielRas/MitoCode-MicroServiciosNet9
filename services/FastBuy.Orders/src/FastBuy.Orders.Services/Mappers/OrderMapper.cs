using FastBuy.Orders.Contracts.DTOs;
using FastBuy.Orders.Repository.Database.Entities;
using FastBuy.Shared.Events.Saga.Orders;

namespace FastBuy.Orders.Services.Mappers
{
    internal static class OrderMapper
    {
        //OrderRequestDto => Order
        public static Order ToEntity(this OrderRequestDto dto)
        {
            var order = new Order { CreateAt = DateTime.UtcNow };

            order.OrderItem = dto.OrderItems.Select(oi => oi.ToEntity(order)).ToList();

            return order;
        }


        //Order => OrderCreatedEvent
        public static OrderCreatedEvent ToOrderCreateEvent(this Order entity, decimal amount)
        {
            var orderItemsForEvent = new List<Shared.Events.Saga.Orders.OrderItem>();

            foreach (var item in entity.OrderItem)
            {
                orderItemsForEvent.Add(
                    new Shared.Events.Saga.Orders.OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                    }
                );
            }

            return new OrderCreatedEvent
            {
                CorrelationId = entity.OrderId,
                OrderId = entity.OrderId,
                Items = orderItemsForEvent,
                CreatedAt = entity.CreateAt,
                Amount = amount
            };
        }


        //OrderItemRequestDto => OrderItem (DB Entity)
        public static Repository.Database.Entities.OrderItem ToEntity(this OrderItemRequestDto dto, Order? order = null)
        {
            var orderItem = new Repository.Database.Entities.OrderItem
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };

            if (order is not null) orderItem.Order = order;

            return orderItem;
        }

    }
}
