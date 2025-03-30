using FastBuy.Products.Entities.Configuration;
using FastBuy.Products.Repositories.Abstractions;
using FastBuy.Products.Repositories.Implementations;
using FastBuy.Products.Services.Abstractions;
using FastBuy.Products.Services.Implementation;
using FastBuy.Shared.Library.Databases;
using FastBuy.Shared.Library.Messaging;

namespace FastBuy.Products.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicactionServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Settings values 
            var serviceSetting = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>()
                ?? throw new ArgumentException($"The {nameof(ServiceSettings)} key has not been configured in the configuration file.");

            var brockerSetting = configuration.GetSection(nameof(BrokerSettings)).Get<BrokerSettings>()
                        ?? throw new ArgumentException($"The {nameof(ServiceSettings)} key has not been configured in the configuration file.");


            //Settings registration
            services.Configure<ServiceSettings>(configuration.GetSection(nameof(ServiceSettings)));

            //MongoDb service registration
            services.AddMongoDb(configuration);

            //MassTransint and RabbitMq registration
            services.AddMessageBroker(configuration);

            //Service registration
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();

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
