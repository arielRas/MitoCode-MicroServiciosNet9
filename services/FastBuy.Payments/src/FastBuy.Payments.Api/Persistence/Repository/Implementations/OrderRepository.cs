using FastBuy.Payments.Api.Entities;
using FastBuy.Payments.Api.Persistence.Repository.Abstractions;

namespace FastBuy.Payments.Api.Persistence.Repository.Implementations
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(PaymentsDbContext context) : base(context)
        {
            
        }
    }
}
