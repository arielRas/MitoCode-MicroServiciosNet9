using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FastBuy.Shared.Library.Repository.Abstractions
{
    public interface IDataBase
    {
        void Configure(IServiceCollection services, IConfiguration configuration);

        void RegisterRepository<T>(IServiceCollection services, string? collectionName = null)
            where T: class, IBaseEntity;
    }
}
