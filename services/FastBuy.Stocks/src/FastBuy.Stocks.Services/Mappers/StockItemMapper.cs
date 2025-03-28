using FastBuy.Stocks.Contracts;
using FastBuy.Stocks.Entities;

namespace FastBuy.Stocks.Services.Mappers
{
    internal static class StockItemMapper
    {
        public static StockResponseDto ToDto(this StockItem entity, ProductInfoDto product)
        {
            return new StockResponseDto
            {
                ProductId = entity.ProductId,
                ProductName = product.Name,
                ProductDescription = product.Description,
                Quantity = entity.Stock,
                LastUpdate = entity.LastUpdate
            };
        }
    }
}
