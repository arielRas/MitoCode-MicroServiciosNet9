using FastBuy.Shared.Library.Configurations;
using FastBuy.Shared.Library.Repository.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using FastBuy.Shared.Library.Repository.Extensions;

namespace FastBuy.Shared.Library.Repository.Implementation.Databases
{
    public class MongoDatabase : IDataBase
    {
        public void Configure(IServiceCollection services, IConfiguration configuration)
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

            services.AddSingleton(serviceProvider =>
                serviceProvider.GetRequiredService<IMongoClient>()
                               .GetDatabase(serviceSetting.ServiceName));
        }

        void IDataBase.RegisterRepository<T>(IServiceCollection services, string? collectionName)
        {
            if(string.IsNullOrWhiteSpace(collectionName))
                throw new ArgumentNullException("The collection name is empty or null.");

            services.AddMongoRepository<T>(collectionName);
        }
    }
}
