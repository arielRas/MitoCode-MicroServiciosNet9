using FastBuy.Payments.Api.DTOs;
using FastBuy.Payments.Api.Entities;

namespace FastBuy.Payments.Api.Mappers
{
    public static class OrderMapper
    {
        //Order => OrderResponseDto
        public static OrderResponseDto ToDto(this Order entity)
        {
            return new OrderResponseDto
            {
                OrderId = entity.OrderId,
                Amount = entity.Amount,
                CreatedAt = entity.CreatedAt,
                Payment = entity.Payment?.ToDto(),                
            };
        }
    }
}
