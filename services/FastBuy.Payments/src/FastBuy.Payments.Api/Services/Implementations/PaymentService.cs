﻿using FastBuy.Payments.Api.DTOs;
using FastBuy.Payments.Api.Entities;
using FastBuy.Payments.Api.Exceptions;
using FastBuy.Payments.Api.Mappers;
using FastBuy.Payments.Api.Persistence.Repository.Abstractions;
using FastBuy.Payments.Api.Services.Abstractions;
using FastBuy.Shared.Events.Saga.Payments;
using MassTransit;

namespace FastBuy.Payments.Api.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IPublishEndpoint _publisher;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(IPaymentRepository paymentRepository, IOrderRepository orderRepository, IPublishEndpoint publisher, ILogger<PaymentService> logger)
        {
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
            _publisher = publisher;
            _logger = logger;
        }

        public async Task<PaymentResponseDto> GetByIdAsync(Guid id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Payment with id {id} does not exist");

            return payment.ToDto();
        }


        public async Task ProcessPaymentAsync(PaymentRequestDto paymentDto)
        {
            var order = await _orderRepository.GetOrderWithPaymentAsync(paymentDto.OrderId)
                    ?? throw new KeyNotFoundException($"The order with id {paymentDto.OrderId} does not exist");

            if (order.Payment!.Status == PaymentStatus.Completed)
                throw new BusinessException("The order you are trying to pay already has a payment processed");

            if (order.Payment!.Status == PaymentStatus.Rejected)
                throw new BusinessException("The order you are trying to pay is canceled due to a previous failed payment.");

            if (order.Amount != paymentDto.Amount)
                throw new BusinessException("The amount entered is different from the amount to be paid");

            await _paymentRepository.ProcessPaymentAsync(order.OrderId, PaymentStatus.Completed);

            await SendSuccessPaymentEvent(order.OrderId);
        }


        private async Task SendSuccessPaymentEvent(Guid correlationId)
        {
            var paymentSuccess = new PaymentSuccessEvent
            {
                CorrelationId = correlationId
            };

            await _publisher.Publish(paymentSuccess, ctx =>
            {
                ctx.CorrelationId = correlationId;
            });

            _logger.LogInformation($"[SAGA] - Send {nameof(PaymentSuccessEvent)} - CorrelationId: {correlationId}");
        }
    }
}
