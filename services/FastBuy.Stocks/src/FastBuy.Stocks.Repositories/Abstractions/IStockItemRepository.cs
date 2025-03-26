using FastBuy.Stocks.Entities;

namespace FastBuy.Stocks.Repositories.Abstractions
{
    public interface IStockItemRepository
    {
        Task<IReadOnlyCollection<StockItem>> GetAllAsync();
        Task<StockItem> GetByIdAsync(Guid id);
        Task<StockItem> CreateAsync(StockItem stockItem);
        Task UpdateAsync(Guid id, StockItem stockItem);
        Task DeleteAsync(Guid id);
    }
}
