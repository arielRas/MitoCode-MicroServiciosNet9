using FastBuy.Stocks.Entities;
using System.Linq.Expressions;

namespace FastBuy.Stocks.Repositories.Abstractions
{
    public interface IStockItemRepository
    {
        Task<IReadOnlyCollection<StockItem>> GetAllAsync();
        Task<StockItem> GetByIdAsync(Guid id);
        Task<StockItem> GetByProductIdAsync(Guid producId);
        Task<StockItem> CreateAsync(StockItem stockItem);
        Task UpdateAsync(Guid id, StockItem stockItem);
    }
}
