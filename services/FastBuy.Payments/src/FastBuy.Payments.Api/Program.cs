
using FastBuy.Payments.Api.Extensions;
using FastBuy.Payments.Api.Middlewares;
using FastBuy.Payments.Api.Routes;

namespace FastBuy.Payments.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApplicationServices(builder.Configuration)
                            .AddPresentation();
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.MapPaymentsRoute();

            app.MapOrdersRoute();

            app.MapControllers();

            app.Run();
        }
    }
}
