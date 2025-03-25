using FastBuy.Products.Entities;

namespace FastBuy.Products.Repositories.Abstractions;

public interface IProductRepository
{
    Task<IReadOnlyCollection<Product>> GetAllAsync();
    Task<Product> GetByIdAsync(Guid id);
    Task<Product> CreateAsync(Product product);
    Task UpdateAsync(Guid id, Product product);
    Task DeleteAsync(Guid id);
}
