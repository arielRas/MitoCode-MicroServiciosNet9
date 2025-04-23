using FastBuy.Orders.Contracts.DTOs;
using FastBuy.Orders.Entities;

namespace FastBuy.Orders.Services.Mappers
{
    internal static class OrderMapper
    {
        public static Order ToEntity(this OrderRequestDto dto, Guid orderId)
        {
            return new Order
            {
                Id = orderId,
                Items = dto.OrderItems.Select(oi => oi.ToEntity(orderId)).ToList(),
                CreatedAt = DateTimeOffset.UtcNow,
                LastUpdate = DateTimeOffset.UtcNow,
                State = "Acepted"
            };
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
