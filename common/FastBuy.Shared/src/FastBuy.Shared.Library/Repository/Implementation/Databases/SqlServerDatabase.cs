using FastBuy.Shared.Library.Configurations;
using FastBuy.Shared.Library.Repository.Abstractions;
using FastBuy.Shared.Library.Repository.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FastBuy.Shared.Library.Repository.Implementation.Databases
{
    class SqlServerDatabase<TContext> : IDataBase where TContext : DbContext
    {
        public void Configure(IServiceCollection services, IConfiguration configuration)
        {
            var sqlServerSettings = configuration.GetSection(nameof(SqlServerSettings)).Get<SqlServerSettings>()
                ?? throw new ArgumentException($"The {nameof(SqlServerSettings)} key has not been configured in the configuration file.");

            services.AddDbContext<TContext>(option =>
                option.UseSqlServer(sqlServerSettings.ConnectionString));
        }

        void IDataBase.RegisterRepository<T>(IServiceCollection services, string? collectionName)
        {
            services.AddSqlServerRepository<T, TContext>();
        }
    }
}
