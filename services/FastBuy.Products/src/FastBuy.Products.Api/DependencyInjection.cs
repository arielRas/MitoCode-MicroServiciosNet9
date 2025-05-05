using FastBuy.Products.Entities;
using FastBuy.Products.Entities.Configuration;
using FastBuy.Products.Services.Abstractions;
using FastBuy.Products.Services.Implementation;
using FastBuy.Shared.Library.Messaging;
using FastBuy.Shared.Library.Repository.Factories;
using FastBuy.Shared.Library.Security;

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

            var dbServiceProvider = configuration["DatabaseProvider"]
                ?? throw new ArgumentException($"The DatabaseProvider key has not been configured in the configuration file.");


            //Settings registration
            services.Configure<ServiceSettings>(configuration.GetSection(nameof(ServiceSettings)));

            //MongoDb service and repositories registration
            var database = DatabaseFactory.CreateDatabase(dbServiceProvider);
            database.Configure(services, configuration);
            database.RegisterRepository<Product>(services, serviceSetting.ServiceName);             

            //MassTransint and RabbitMq registration
            services.AddMessageBroker(configuration);

            //Authorization registration
            services.AddJwtBearerAuthentication();

            //Service registration            
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
