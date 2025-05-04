using FastBuy.Orders.Repository.Database.Entities;

namespace FastBuy.Orders.Repository.Repositories.Abstractions
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<decimal> GetOrderAmountAsync(Guid orderId);
        Task<IEnumerable<OrderItem>> GetOrderItemAsync(Guid orderId);
    }
}
