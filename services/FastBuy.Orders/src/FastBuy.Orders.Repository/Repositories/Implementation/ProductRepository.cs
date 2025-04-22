using FastBuy.Orders.Entities;
using FastBuy.Orders.Repository.Database;
using FastBuy.Orders.Repository.Repositories.Abstractions;

namespace FastBuy.Orders.Repository.Repositories.Implementation
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(OrdersDbContext context) : base(context)
        {
            
        }

        public async Task UpdateAsync(Product product)
        {
            var existingEntity = await dbSet.FindAsync(product.Id)
                ?? throw new KeyNotFoundException($"The resource with id {product.Id} does not exist");

            existingEntity.Price = product.Price;
            existingEntity.Name = product.Name;
            existingEntity.Description = product.Description;

            await _context.SaveChangesAsync();
        }
    }
}
