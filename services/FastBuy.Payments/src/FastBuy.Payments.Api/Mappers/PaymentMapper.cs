using FastBuy.Payments.Api.DTOs;
using FastBuy.Payments.Api.Entities;
using FastBuy.Shared.Events.Saga.Payments;

namespace FastBuy.Payments.Api.Mappers
{
    public static class PaymentMapper
    {
        //PaymentRequestDto => Payment
        public static Payment ToEntity(this PaymentRequestDto dto, string status)
        {
            return new Payment
            {
                OrderId = dto.OrderId,
                OrderDate = DateTimeOffset.Now,
                Status = status,
            };
        }
        

        //Payment => PaymentResponseDto
        public static PaymentResponseDto ToDto(this Payment entity)
        {
            return new PaymentResponseDto
            {
                PaymentId = entity.OrderId,
                CreatedAt = entity.OrderDate,
                Status = entity.Status
            };
        }


        
    }
}
