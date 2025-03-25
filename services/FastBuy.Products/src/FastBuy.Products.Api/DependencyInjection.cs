using FastBuy.Products.Entities.Configuration;
using FastBuy.Products.Repositories.Abstractions;
using FastBuy.Products.Repositories.Implementations;
using FastBuy.Products.Services.Abstractions;
using FastBuy.Products.Services.Implementation;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace FastBuy.Products.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicactionServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Settings registration
            services.Configure<ServiceSettings>(configuration.GetSection(nameof(ServiceSettings)));

            //MongoDb Serializer
            BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(MongoDB.Bson.BsonType.String));

            //MongoDb Service registration
            services.AddSingleton<IMongoClient>(serviceProvider =>
                new MongoClient(configuration.GetConnectionString("DefaultConnection")));

            services.AddSingleton<IMongoDatabase>(serviceProvider =>
            {
                var settings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>()
                    ?? throw new ArgumentException($"The {nameof(ServiceSettings)} key has not been configured in the configuration file.");

                return serviceProvider.GetRequiredService<IMongoClient>()
                                      .GetDatabase(settings.ServiceName);
            });

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
