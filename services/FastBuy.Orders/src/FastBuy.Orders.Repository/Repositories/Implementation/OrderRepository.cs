using FastBuy.Orders.Repository.Database;
using FastBuy.Orders.Repository.Database.Entities;
using FastBuy.Orders.Repository.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace FastBuy.Orders.Repository.Repositories.Implementation
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(OrdersDbContext context) : base(context)
        {

        }

        public async Task<decimal> GetOrderAmountAsync(Guid orderId)
        {
            return await dbSet.Include(o => o.OrderItem)
                                .ThenInclude(oi => oi.Product)
                              .Where(o => o.OrderId == orderId)
                              .SelectMany(o => o.OrderItem)
                              .SumAsync(oi => oi.Product.Price * oi.Quantity);
        }
    }
}
