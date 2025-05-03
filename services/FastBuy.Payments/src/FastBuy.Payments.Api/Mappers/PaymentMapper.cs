using FastBuy.Payments.Api.DTOs;
using FastBuy.Payments.Api.Entities;

namespace FastBuy.Payments.Api.Mappers
{
    public static class PaymentMapper
    {
        //PaymentRequestDto => Payment
        public static Payment ToEntity(this PaymentRequestDto dto)
        {
            return new Payment
            {
                OrderId = dto.OrderId,
                CreatedAt = DateTimeOffset.Now,
                Status = "OK"
            };
        }

        //Payment => PaymentResponseDto
        public static PaymentResponseDto ToDto(this Payment entity)
        {
            return new PaymentResponseDto
            {
                PaymentId = entity.OrderId,
                CreatedAt = entity.CreatedAt,
                Status = entity.Status
            };
        }
    }
}
