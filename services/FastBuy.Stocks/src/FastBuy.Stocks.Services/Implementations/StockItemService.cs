using FastBuy.Stocks.Contracts;
using FastBuy.Stocks.Entities;
using FastBuy.Stocks.Repositories.Abstractions;
using FastBuy.Stocks.Services.Abstractions;
using FastBuy.Stocks.Services.Mappers;
using Microsoft.Extensions.Logging;

namespace FastBuy.Stocks.Services.Implementations
{
    public class StockItemService : IStockItemService
    {
        private readonly IStockItemRepository _repository;
        private readonly ILogger<StockItemService> _logger;

        public StockItemService(IStockItemRepository repository, ILogger<StockItemService> logger)
        {
            _repository = repository;
            _logger = logger;
        }


        public async Task<StockResponseDto> GetByProductIdAsync(Guid productId, ProductInfoDto productInfo)
        {
            var stockItem = await _repository.GetByProductIdAsync(productId)
                ?? throw new KeyNotFoundException($"The resource with id {productId} does not exist"); ;

            return stockItem.ToDto(productInfo);
        }


        public async Task<bool> SetStockAsync(Guid productId, int stock)
        {

            var stockItem = await _repository.GetByProductIdAsync(productId);

            if (stockItem is null)
            {
                stockItem = new StockItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = productId,
                    Stock = stock,
                    LastUpdate = DateTime.UtcNow
                };

                await _repository.CreateAsync(stockItem);
            }
            else
            {  
                stockItem.Stock = stock;
                stockItem.LastUpdate = DateTime.UtcNow;
                await _repository.UpdateAsync(productId, stockItem);
            }

            return true;
        }


        public async Task<bool> DecreaseStockAsync(StockDecreaseRequestDto stockDecreaseDto)
        {
            var stockItem = await _repository.GetByProductIdAsync(stockDecreaseDto.ProductId);

            if(stockItem is null) return false;

            if(stockItem.Stock < stockDecreaseDto.Quantity) return false;

            stockItem.Stock -= stockDecreaseDto.Quantity;

            stockItem.LastUpdate = DateTimeOffset.UtcNow;

            await _repository.UpdateStockAsync(stockItem);

            return true;
        }
    }
}
