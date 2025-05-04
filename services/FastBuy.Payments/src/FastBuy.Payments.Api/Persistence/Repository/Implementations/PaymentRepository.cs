using FastBuy.Payments.Api.Entities;
using FastBuy.Payments.Api.Persistence.Repository.Abstractions;

namespace FastBuy.Payments.Api.Persistence.Repository.Implementations
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(PaymentsDbContext context) : base(context)
        {
            
        }

        public async Task ProcessPaymentAsync(Guid orderId, string paymentStatus)
        {
            var payment = await dbSet.FindAsync(orderId)
                ?? throw new KeyNotFoundException($"Payment with id {orderId} does not exist");

            payment.LastUpdate = DateTime.UtcNow;

            payment.Status = paymentStatus;

            await _context.SaveChangesAsync();
        }
    }
}
