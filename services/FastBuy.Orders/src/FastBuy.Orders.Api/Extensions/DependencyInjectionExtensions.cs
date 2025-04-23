using FastBuy.Orders.Repository.Database;
using FastBuy.Shared.Library.Configurations;
using FastBuy.Shared.Library.Repository.Factories;
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
