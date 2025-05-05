using FastBuy.Payments.Api.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace FastBuy.Payments.Api.Routes
{
    public static class OrdersRoutes
    {
        public static IApplicationBuilder MapOrdersRoute(this WebApplication app)
        {
            var group = app.MapGroup("/api/orders")
                           .RequireAuthorization(policy => policy.RequireRole("Admin"))
                           .WithTags("Orders");

            group.MapGet("/{id:Guid}", GetById);

            return app;
        }

        private static async Task<IResult> GetById([FromRoute] Guid id, IOrderService service)
        {
            try
            {
                var order = await service.GetOrderAsync(id);

                return Results.Ok(order);
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
        }
    }
}
