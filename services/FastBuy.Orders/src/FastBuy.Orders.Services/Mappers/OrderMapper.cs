using FastBuy.Orders.Contracts.DTOs;
using FastBuy.Orders.Entities;

namespace FastBuy.Orders.Services.Mappers
{
    internal static class OrderMapper
    {
        public static Order ToEntity(this OrderRequestDto dto, Guid orderId)
        {
            var order =  new Order
            {
                Id = orderId,
                Items = dto.OrderItems.Select(oi => oi.ToEntity(orderId)).ToList(),
                CreatedAt = DateTimeOffset.UtcNow,
                LastUpdate = DateTimeOffset.UtcNow,
                State = "Acepted"
            };

            foreach (var item in order.Items)
            {
                item.Order = order;
            }

            return order;
        }


        public static OrderItem ToEntity(this OrderItemRequestDto dto, Guid orderId)
        {
            return new OrderItem
            {
                OrderId = orderId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };
        }
    }
}
