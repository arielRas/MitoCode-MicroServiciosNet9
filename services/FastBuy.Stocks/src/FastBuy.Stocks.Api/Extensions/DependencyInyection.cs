using FastBuy.Shared.Events.Exceptions;
using FastBuy.Shared.Library.Configurations;
using FastBuy.Shared.Library.Messaging;
using FastBuy.Shared.Library.Repository.Factories;
using FastBuy.Shared.Library.Security;
using FastBuy.Stocks.Entities;
using FastBuy.Stocks.Services.Abstractions;
using FastBuy.Stocks.Services.Clients;
using FastBuy.Stocks.Services.Consumers;
using FastBuy.Stocks.Services.Exceptions;
using FastBuy.Stocks.Services.Implementations;
using MassTransit;

namespace FastBuy.Stocks.Api.Extensions
{
    public static class DependencyInyection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Settings values 
            var serviceSetting = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>()
                ?? throw new ArgumentException($"The {nameof(ServiceSettings)} key has not been configured in the configuration file.");

            var brockerSetting = configuration.GetSection(nameof(BrokerSettings)).Get<BrokerSettings>()
                        ?? throw new ArgumentException($"The {nameof(BrokerSettings)} key has not been configured in the configuration file.");

            var dbServiceProvider = configuration["DatabaseProvider"]
                ?? throw new ArgumentException($"The DatabaseProvider key has not been configured in the configuration file.");


            //Settings registration
            services.Configure<ServiceSettings>(configuration.GetSection(nameof(ServiceSettings)));


            //MonogDb service registration
            var database = DatabaseFactory.CreateDatabase(dbServiceProvider);
            database.Configure(services, configuration);
            database.RegisterRepository<StockItem>(services, serviceSetting.ServiceName);
            database.RegisterRepository<ProductItem>(services, "Products");


            //MassTransit service registration
            services.AddMessageBroker(
                configuration,
                typeof(ProductItemDeleteConsumer).Assembly,
                retryConfigurator =>
                {
                    retryConfigurator.Interval(3, TimeSpan.FromSeconds(4));
                    retryConfigurator.Ignore(typeof(AsynchronousMessagingException));
                });
            

            //HttpClients registration and Policy application
            services.AddHttpClient<ProductsClient>(client =>
            {
                client.BaseAddress = new Uri(configuration.GetSection("Urls:ProductsUrl").Value
                    ?? throw new ArgumentException($"The ProductsUrl key has not been configured in the configuration file."));
            })
            .AddResiliencePolicies();


            //Jwt configure and register
            services.AddJwtBearerAuthentication();            


            //Service registration
            services.AddScoped<IStockItemService, StockItemService>();

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
