using FastBuy.Orders.Entities.Settings;
using FastBuy.Orders.Repository.Database;
using FastBuy.Orders.Repository.Repositories.Abstractions;
using FastBuy.Orders.Repository.Repositories.Implementation;
using FastBuy.Orders.Services.Abstractions;
using FastBuy.Orders.Services.Implementations;
using Microsoft.EntityFrameworkCore;

namespace FastBuy.Orders.Api.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Settings values
            var serviceSetting = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>()
                ?? throw new ArgumentException($"The {nameof(ServiceSettings)} key has not been configured in the configuration file.");

            //Database registration
            services.AddDbContext<OrdersDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });


            //Broker Message registration
            services.AddMessageBroker(configuration);


            //Authentication configure and register
            services.AddJwtBearerAuthentication(configuration);


            //Services and Repository registration
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();


            return services;
        }

        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            //Controller registration
            services.AddControllers();

            //configure Swagger as an endpoint explorer
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
    }
}
