using FastBuy.Payments.Api.DTOs;
using FastBuy.Payments.Api.Exceptions;
using FastBuy.Payments.Api.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace FastBuy.Payments.Api.Routes
{
    public static class PaymentsRoute
    {
        public static IApplicationBuilder MapPaymentsRoute(this WebApplication app)
        {
            var group = app.MapGroup("/api/payments").WithTags("Payments");

            group.MapPost("", Create);

            return app;
        }

        private static async Task<IResult> Create([FromBody] PaymentRequestDto dto, IPaymentService service)
        {
            try
            {
                await service.CreateAsync(dto);

                return Results.Accepted();
            }
            catch(KeyNotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (BusinessException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }
    }
}
