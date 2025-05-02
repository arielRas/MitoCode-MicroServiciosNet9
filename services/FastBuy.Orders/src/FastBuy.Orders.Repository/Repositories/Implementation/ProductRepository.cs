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
    }
}
