using FastBuy.Orders.Entities;
using FastBuy.Orders.Repository.Database;
using FastBuy.Orders.Repository.Repositories.Abstractions;

namespace FastBuy.Orders.Repository.Repositories.Implementation
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(OrdersDbContext context) : base(context)
        {

        }
    }
}
