using FastBuy.Payments.Api.Consumers;
using FastBuy.Payments.Api.Persistence;
using FastBuy.Payments.Api.Persistence.Repository.Abstractions;
using FastBuy.Payments.Api.Persistence.Repository.Implementations;
using FastBuy.Payments.Api.Services.Abstractions;
using FastBuy.Payments.Api.Services.Implementations;
using FastBuy.Shared.Events.Exceptions;
using FastBuy.Shared.Library.Configurations;
using FastBuy.Shared.Library.Messaging;
using FastBuy.Shared.Library.Repository.Factories;
using MassTransit;

namespace FastBuy.Payments.Api.Extensions
{
    public static class DependencyInyectionExtensions
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


            //DataBase configuration
            var database = DatabaseFactory.CreateDatabase<PaymentsDbContext>(dbServiceProvider);
            database.Configure(services, configuration);


            //BrokerMessage register
            services.AddMessageBroker(
                configuration,
                typeof(OrderCreateConsumer).Assembly,
                retryConfigurator =>
                {
                    retryConfigurator.Interval(3, TimeSpan.FromSeconds(4));
                    retryConfigurator.Ignore(typeof(AsynchronousMessagingException));
                }
             );


            //Services register
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPaymentService, PaymentService>();


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
