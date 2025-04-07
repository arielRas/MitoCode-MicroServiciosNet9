using FastBuy.Stocks.Contracts;
using FastBuy.Stocks.Entities;

namespace FastBuy.Stocks.Services.Mappers
{
    internal static class StockItemMapper
    {
        public static StockResponseDto ToDto(this StockItem stock, ProductItem product)
        {
            return new StockResponseDto
            {
                ProductId = stock.ProductId,
                ProductName = product.Name!,
                ProductDescription = product.Description!,
                Quantity = stock.Stock,
                LastUpdate = stock.LastUpdate
            };
        }
    }
}
