using FastBuy.Shared.Library.Repository.Abstractions;
using FastBuy.Shared.Library.Repository.Implementation.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FastBuy.Shared.Library.Repository.Extensions
{
    public static class SqlServerExtensions
    {
        public static IServiceCollection AddSqlServerRepository<T, TContext>(this IServiceCollection services)
            where T : class, IBaseEntity
            where TContext : DbContext
        {
            services.TryAddScoped<IRepository<T>>(serviceProvider =>
            {
                var context = serviceProvider.GetRequiredService<TContext>();

                return new SqlServerRepository<T>(context);
            });

            return services;
        }
    }
}
