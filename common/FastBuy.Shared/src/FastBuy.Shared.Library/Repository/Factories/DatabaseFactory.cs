using FastBuy.Shared.Library.Repository.Abstractions;
using FastBuy.Shared.Library.Repository.Implementation.Databases;
using Microsoft.EntityFrameworkCore;

namespace FastBuy.Shared.Library.Repository.Factories
{
    public static class DatabaseFactory
    {
        public static IDataBase CreateDatabase(string provider)
        {
            var normalizedProvider = provider.ToLower().Trim();

            return normalizedProvider switch
            {
                "mongodb" => new MongoDatabase(),
                _ => throw new NotImplementedException($"The {provider} database provider is not implemented")
            };
        }

        public static IDataBase CreateDatabase<TContext>(string provider) where TContext : DbContext
        {
            var normalizedProvider = provider.ToLower().Trim();

            return normalizedProvider switch
            {
                "sqlserver" => new SqlServerDatabase<TContext>(),
                _ => throw new NotImplementedException($"The {provider} database provider is not implemented")
            };
        }
    }
}
