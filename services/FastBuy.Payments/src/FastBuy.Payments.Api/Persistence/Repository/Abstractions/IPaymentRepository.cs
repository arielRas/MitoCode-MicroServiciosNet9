using FastBuy.Payments.Api.Entities;

namespace FastBuy.Payments.Api.Persistence.Repository.Abstractions
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task ProcessPaymentAsync(Guid orderId, string paymentStatus);
    }
}
