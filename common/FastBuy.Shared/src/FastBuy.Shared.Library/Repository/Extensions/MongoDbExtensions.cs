using FastBuy.Shared.Library.Repository.Abstractions;
using FastBuy.Shared.Library.Repository.Implementation.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;

namespace FastBuy.Shared.Library.Repository.Extensions
{
    public static class MongoDbExtensions
    {
        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) 
            where T : class, IBaseEntity
        {
            services.TryAddSingleton<IRepository<T>>(serviceProvider =>
            {
                var database = serviceProvider.GetRequiredService<IMongoDatabase>();

                return new MongoDbRepository<T>(database, collectionName);
            });

            return services;
        }
    }
}
