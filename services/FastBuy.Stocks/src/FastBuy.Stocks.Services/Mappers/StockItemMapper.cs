using FastBuy.Stocks.Contracts;
using FastBuy.Stocks.Entities;

namespace FastBuy.Stocks.Services.Mappers
{
    internal static class StockItemMapper
    {
        public static StockResponseDto ToDto(this StockItem entity)
        {
            return new StockResponseDto
            {
                ProductId = entity.ProductId,
                Quantity = entity.Stock,
                LastUpdate = entity.LastUpdate
            };
        }
    }
}
