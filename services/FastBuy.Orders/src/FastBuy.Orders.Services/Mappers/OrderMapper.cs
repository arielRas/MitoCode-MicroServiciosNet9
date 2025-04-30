using FastBuy.Orders.Contracts.DTOs;
using FastBuy.Orders.Entities;
using FastBuy.Shared.Events.Saga.Stocks;

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


        public static Entities.OrderItem ToEntity(this OrderItemRequestDto dto, Guid orderId)
        {
            return new Entities.OrderItem
            {
                OrderId = orderId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };
        }

        public static StockDecreaseRequestedEvent ToStockDecreaseEvent(this Order entity, Guid correlationId)
        {
            var orderItemsForEvent = new List<Shared.Events.Saga.Orders.OrderItem>();

            foreach (var item in entity.Items)
            {
                orderItemsForEvent.Add(
                    new Shared.Events.Saga.Orders.OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    }
                );
            }

            return new StockDecreaseRequestedEvent
            {
                CorrelationId = correlationId,
                Items = orderItemsForEvent
            };
        }
    }
}