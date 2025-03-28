using FastBuy.Stocks.Entities.Configuration;
using FastBuy.Stocks.Repositories.Abstractions;
using FastBuy.Stocks.Repositories.Implementations;
using FastBuy.Stocks.Services.Abstractions;
using FastBuy.Stocks.Services.Clients;
using FastBuy.Stocks.Services.Implementations;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace FastBuy.Stocks.Api
{
    public static class DependencyInyection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Settings registration
            services.Configure<ServiceSettings>(configuration.GetSection(nameof(ServiceSettings)));

            //MongoDb Serializaers           
            BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(MongoDB.Bson.BsonType.String));

            //MonogDb service registration
            services.AddSingleton<IMongoClient>(serviceProvider =>
                new MongoClient(configuration.GetConnectionString("DefaultConnection")));

            services.AddSingleton<IMongoDatabase>(serviceProvider =>
            {
                var settings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>()
                    ?? throw new ArgumentException($"The {nameof(ServiceSettings)} key has not been configured in the configuration file.");

                return serviceProvider.GetRequiredService<IMongoClient>()
                                      .GetDatabase(settings.ServiceName);
            });

            //Recielence policy
            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(3), TimeoutStrategy.Pessimistic);

            var randomJitter = new Random();

            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAtempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAtempt)) +
                        TimeSpan.FromMicroseconds(randomJitter.Next(0,100)),
                    onRetry: (outcome, timespan, retryAttempt, context) =>
                        Console.WriteLine($"[RETRY] Attempt {retryAttempt} failed, retrying in {timespan.TotalSeconds} seconds")                
                );

            //HttpClients registration
            services.AddHttpClient<ProductsClient>(client =>
            {
                client.BaseAddress = new Uri(configuration.GetSection("Urls:ProductsUrl").Value 
                    ?? throw new ArgumentException($"The ProductsUrl key has not been configured in the configuration file."));
            })
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(timeoutPolicy);


            //Service registration
            services.AddScoped<IStockItemRepository, StockItemRepository>();
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
