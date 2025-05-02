using FastBuy.Payments.Api.Persistence;
using FastBuy.Shared.Library.Configurations;
using FastBuy.Shared.Library.Repository.Factories;

namespace FastBuy.Payments.Api.Extensions
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


            //DataBase configuration
            var database = DatabaseFactory.CreateDatabase<PaymentsDbContext>(dbServiceProvider);
            database.Configure(services, configuration);



            return services;
        }
    }
}
