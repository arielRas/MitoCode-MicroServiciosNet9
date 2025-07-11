﻿using FastBuy.Orders.Contracts.DTOs;
using FastBuy.Orders.Repository.Database.Entities;
using FastBuy.Shared.Events.Saga.Orders;

namespace FastBuy.Orders.Services.Mappers
{
    internal static class OrderMapper
    {
        //Order => OrderResponseDto
        public static OrderResponseDto ToDto(this Order entity)
        {
            return new OrderResponseDto
            {
                Id = entity.OrderId,
                State = String.Empty,
                OrderItems = entity.OrderItem.Select(oi => oi.ToDto()).ToList(),
                CreatedAt = entity.CreateAt,
                Amount = entity.OrderItem.Sum(oi => oi.Quantity * oi.Product.Price)
            };
        }

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

        //OrderItem(Entity) => OrderItemResponseDto
        public static OrderItemResponseDto ToDto(this Repository.Database.Entities.OrderItem entity)
        {
            return new OrderItemResponseDto
            {
                ProductId = entity.ProductId,
                ProductName = entity.Product.Name,
                Quantity = entity.Quantity,
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
            
        //OrderItem(Entity) => OrderItem(Event)
        public static Shared.Events.Saga.Orders.OrderItem ToEvent(this Repository.Database.Entities.OrderItem entity)
        {
            return new Shared.Events.Saga.Orders.OrderItem
            {
                ProductId = entity.ProductId,
                Quantity = entity.Quantity
            };
        }
    }
}
