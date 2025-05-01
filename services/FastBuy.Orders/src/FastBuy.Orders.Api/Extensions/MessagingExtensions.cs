using FastBuy.Orders.Entities.Settings;
using FastBuy.Orders.Repository.Database;
using FastBuy.Orders.Repository.Saga;
using FastBuy.Orders.Services.Consumers;
using FastBuy.Orders.Services.StateMachines;
using FastBuy.Shared.Events.Exceptions;
using MassTransit;
using System.Reflection;

namespace FastBuy.Orders.Api.Extensions
{
    public static class MessagingExtensions
    {
        public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
        {
            var brokerSettings = configuration.GetSection(nameof(BrokerSettings)).Get<BrokerSettings>()
                ?? throw new ArgumentException($"The {nameof(BrokerSettings)} key has not been configured in the configuration file.");

            var serviceSetting = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>()
                ?? throw new ArgumentException($"The {nameof(ServiceSettings)} key has not been configured in the configuration file.");

            services.AddMassTransit(configure =>
            {                
                configure.AddConsumers(typeof(ProductChangeConsumer).Assembly);

                configure.AddConsumers(Assembly.GetEntryAssembly()); //Self-assembly register for the SAGA pattern

                configure.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(brokerSettings.Host);

                    configurator.ConfigureEndpoints(
                        context,
                        new KebabCaseEndpointNameFormatter(
                            prefix: serviceSetting.ServiceName,
                            includeNamespace: false
                        )
                    );

                    configurator.UseMessageRetry(retryConfigurator =>
                    {
                        retryConfigurator.Interval(3, TimeSpan.FromSeconds(4));
                        retryConfigurator.Ignore(typeof(AsynchronousMessagingException));
                    });
                });

                configure.AddSagaStateMachine<OrderStateMachine, OrderState>()
                         .EntityFrameworkRepository(configurator =>
                         {
                             configurator.ConcurrencyMode = ConcurrencyMode.Optimistic;
                             configurator.ExistingDbContext<OrdersDbContext>();
                         });
            });

            return services;
        }
    }
}
