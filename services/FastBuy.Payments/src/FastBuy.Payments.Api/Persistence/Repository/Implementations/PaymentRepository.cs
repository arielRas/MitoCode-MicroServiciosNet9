using FastBuy.Payments.Api.Entities;
using FastBuy.Payments.Api.Persistence.Repository.Abstractions;

namespace FastBuy.Payments.Api.Persistence.Repository.Implementations
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(PaymentsDbContext context) : base(context)
        {
            
        }
    }
}
