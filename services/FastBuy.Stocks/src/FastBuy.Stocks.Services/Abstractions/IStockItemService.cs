using FastBuy.Stocks.Contracts;

namespace FastBuy.Stocks.Services.Abstractions
{
    public interface IStockItemService
    {
        Task<IEnumerable<StockResponseDto>> GetAllAsync();
        Task<StockResponseDto> GetByIdAsync(Guid id);
        Task<StockResponseDto> GetByProductIdAsync(Guid productId);
        Task<bool> SetStockAsync(Guid productId, int stock);
        Task<bool> DecreaseStockAsync(StockDecreaseRequestDto stockDecreaseDto);
    }
}
