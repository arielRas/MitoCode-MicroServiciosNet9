using FastBuy.Orders.Entities;

namespace FastBuy.Orders.Repository.Repositories.Abstractions
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<decimal> GetOrderAmountAsync(Guid orderId);
    }
}
