using FastBuy.Shared.Library.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace FastBuy.Shared.Library.Databases
{
    public static class MongoDbExtensions
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceSetting = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>()
                ?? throw new ArgumentException($"The {nameof(ServiceSettings)} key has not been configured in the configuration file.");

            var mongoSetting = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>()
                ?? throw new ArgumentException($"The {nameof(MongoDbSettings)} key has not been configured in the configuration file.");

            //MongoDb Serializer
            BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(MongoDB.Bson.BsonType.String));

            //MongoDb Service registration
            services.AddSingleton<IMongoClient>(serviceProvider =>
                new MongoClient(mongoSetting.ConnectionString));

            services.AddSingleton<IMongoDatabase>(serviceProvider =>
                serviceProvider.GetRequiredService<IMongoClient>()
                               .GetDatabase(serviceSetting.ServiceName));

            return services;
        }
    }
}
