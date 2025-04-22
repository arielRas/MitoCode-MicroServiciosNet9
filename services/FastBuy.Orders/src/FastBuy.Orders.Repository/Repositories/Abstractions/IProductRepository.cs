using FastBuy.Orders.Entities;

namespace FastBuy.Orders.Repository.Repositories.Abstractions
{
    public interface IProductRepository : IRepository<Product>
    {
        Task UpdateAsync(Product product);
    }
}
