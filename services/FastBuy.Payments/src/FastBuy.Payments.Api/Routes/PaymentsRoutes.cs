using FastBuy.Payments.Api.DTOs;
using FastBuy.Payments.Api.Exceptions;
using FastBuy.Payments.Api.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace FastBuy.Payments.Api.Routes
{
    public static class PaymentsRoutes
    {
        public static IApplicationBuilder MapPaymentsRoute(this WebApplication app)
        {
            var group = app.MapGroup("/api/payments")
                           .RequireAuthorization(policy => policy.RequireRole("Admin"))
                           .WithTags("Payments");

            group.MapPatch("", ProcessPayment);

            group.MapGet("/{id:Guid}", GetById);

            return app;
        }

        private static async Task<IResult> ProcessPayment([FromBody] PaymentRequestDto dto, IPaymentService service)
        {
            await service.ProcessPaymentAsync(dto);

            return Results.Accepted();
        }

        private static async Task<IResult> GetById([FromRoute] Guid id, IPaymentService service)
        {
            var payment = await service.GetByIdAsync(id);

            return Results.Ok(payment);
        }
    }
}
