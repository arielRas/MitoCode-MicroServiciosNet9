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
            IEnumerable<Assembly>? consumerAssembly = null,
            Action<IRetryConfigurator>? retryConfigurator = null
            )
        {
            //Settings values 
            var serviceSetting = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>()
                ?? throw new ArgumentException($"The {nameof(ServiceSettings)} key has not been configured in the configuration file.");

            var brokerSetting = configuration.GetSection(nameof(BrokerSettings)).Get<BrokerSettings>()
                        ?? throw new ArgumentException($"The {nameof(BrokerSettings)} key has not been configured in the configuration file.");

            if (retryConfigurator is null)
                retryConfigurator = configurator => configurator.Interval(3, TimeSpan.FromSeconds(4));

            //MassTransint and RabbitMq registration
            services.AddMassTransit(configure =>
            {
                if (consumerAssembly is not null && consumerAssembly.Any())
                    consumerAssembly.ToList().ForEach(a => configure.AddConsumers(a));

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

                    configurator.UseMessageRetry(retryConfigurator);
                });
            });

            return services;
        }
    }
}
