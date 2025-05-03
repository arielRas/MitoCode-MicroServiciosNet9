using FastBuy.Payments.Api.Entities;

namespace FastBuy.Payments.Api.Persistence.Repository.Abstractions
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order> GetOrderWithPaymentAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
