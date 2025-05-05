using FastBuy.Orders.Repository.Database.Entities;

namespace FastBuy.Orders.Repository.Repositories.Abstractions
{
    public interface IProductRepository : IRepository<Product>
    {
        Task SetProductActiveAsync(Guid id, bool isActive);
    }
}
