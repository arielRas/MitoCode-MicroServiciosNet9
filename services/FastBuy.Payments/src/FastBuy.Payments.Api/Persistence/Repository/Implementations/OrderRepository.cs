using FastBuy.Payments.Api.Entities;
using FastBuy.Payments.Api.Persistence.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace FastBuy.Payments.Api.Persistence.Repository.Implementations
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(PaymentsDbContext context) : base(context)
        {
            
        }

        public async Task<Order> GetOrderWithPaymentAsync(Guid id)
        {
            return await dbSet.Include(o => o.Payment)
                              .Where(o => o.OrderId == id)
                              .FirstOrDefaultAsync()
                              ?? throw new KeyNotFoundException($"Order with id {id} does not exist");
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await dbSet.AnyAsync(o => o.OrderId == id);
        }
    }
}
