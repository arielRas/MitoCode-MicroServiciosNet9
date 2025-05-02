using FastBuy.Orders.Repository.Database;
using FastBuy.Orders.Repository.Database.Entities;
using FastBuy.Orders.Repository.Repositories.Abstractions;

namespace FastBuy.Orders.Repository.Repositories.Implementation
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(OrdersDbContext context) : base(context)
        {
            
        }

        public async Task SetProductActiveAsync(Guid id, bool isActive)
        {
            var product = await dbSet.FindAsync(id)
                ?? throw new KeyNotFoundException($"The product with ID {id} does not exist");

            product.IsActive = isActive;
        }
    }
}
