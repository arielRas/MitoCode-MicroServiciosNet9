using FastBuy.Shared.Library.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using System.Reflection;

namespace FastBuy.Shared.Library.Messaging
{
    public static class MassTransitExtensions
    {
        public static IServiceCollection AddMessageBroker(
            this IServiceCollection services,
            IConfiguration configuration,
            Assembly? consumerAssembly = null)
        {
            //Settings values 
            var serviceSetting = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>()
                ?? throw new ArgumentException($"The {nameof(ServiceSettings)} key has not been configured in the configuration file.");

            var brokerSetting = configuration.GetSection(nameof(BrokerSettings)).Get<BrokerSettings>()
                        ?? throw new ArgumentException($"The {nameof(BrokerSettings)} key has not been configured in the configuration file.");

            //MassTransint and RabbitMq registration
            services.AddMassTransit(configure =>
            {
                if (consumerAssembly is not null)
                    configure.AddConsumers(consumerAssembly);

                configure.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(brokerSetting.Host);

                    configurator.ConfigureEndpoints(
                        context,
                        new KebabCaseEndpointNameFormatter(
                            prefix: serviceSetting.ServiceName,
                            includeNamespace: false
                        )
                    );

                });
            });

            return services;
        }
    }
}
