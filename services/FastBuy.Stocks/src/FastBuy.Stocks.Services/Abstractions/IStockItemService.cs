using FastBuy.Stocks.Contracts.DTOs;

namespace FastBuy.Stocks.Services.Abstractions
{
    public interface IStockItemService
    {       
        Task<StockResponseDto> GetByProductIdAsync(Guid productId);
        Task<bool> SetStockAsync(Guid productId, int stock);
        Task<bool> DecreaseStockAsync(StockDecreaseRequestDto stockDecreaseDto);
    }
}
