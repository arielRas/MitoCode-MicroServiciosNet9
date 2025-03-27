using FastBuy.Stocks.Entities;

namespace FastBuy.Stocks.Repositories.Abstractions
{
    public interface IStockItemRepository
    {
        Task<IReadOnlyCollection<StockItem>> GetAllAsync();
        Task<StockItem> GetByProductIdAsync(Guid producId);
        Task<StockItem> CreateAsync(StockItem stockItem);
        Task UpdateAsync(Guid id, StockItem stockItem);
        Task UpdateStockAsync(StockItem stockItem);
    }
}
