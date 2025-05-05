
using FastBuy.Orders.Api.Extensions;
using FastBuy.Orders.Api.Middlewares;

namespace FastBuy.Orders.Api
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

            app.MapControllers();

            app.Run();
        }
    }
}
