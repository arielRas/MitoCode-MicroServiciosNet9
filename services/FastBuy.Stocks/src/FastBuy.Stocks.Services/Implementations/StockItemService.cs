using FastBuy.Shared.Library.Repository.Abstractions;
using FastBuy.Stocks.Contracts;
using FastBuy.Stocks.Entities;
using FastBuy.Stocks.Services.Abstractions;
using FastBuy.Stocks.Services.Mappers;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace FastBuy.Stocks.Services.Implementations
{
    public class StockItemService : IStockItemService
    {
        private readonly IRepository<StockItem> _stockRepository;
        private readonly IRepository<ProductItem> _productRepository;
        private readonly ILogger<StockItemService> _logger;

        public StockItemService(IRepository<StockItem> stockRepository, IRepository<ProductItem> productRepository, ILogger<StockItemService> logger)
        {
            _stockRepository = stockRepository;
            _productRepository = productRepository;
            _logger = logger;
        }


        public async Task<StockResponseDto> GetByProductIdAsync(Guid productId)
        {
            Expression<Func<StockItem, bool>> filter = x => x.ProductId == productId;

            var stockItem = await _stockRepository.GetAsync(filter);

            var productItem = await _productRepository.GetByIdAsync(productId);

            return stockItem.ToDto(productItem);
        }


        public async Task<bool> SetStockAsync(Guid productId, int stock)
        {
            Expression<Func<StockItem, bool>> filter = x => x.ProductId == productId;

            if (await _stockRepository.ExistAsync(filter))
            {
                var stockItem = await _stockRepository.GetAsync(filter);
                
                stockItem.Stock = stock;
                
                stockItem.LastUpdate = DateTime.UtcNow;
                
                await _stockRepository.UpdateAsync(stockItem.Id, stockItem);
            }
            else
            {
                var stockItem = new StockItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = productId,
                    Stock = stock,
                    LastUpdate = DateTime.UtcNow
                };

                await _stockRepository.CreateAsync(stockItem);
            }

            return true;
        }


        public async Task<bool> DecreaseStockAsync(StockDecreaseRequestDto stockDecreaseDto)
        {
            Expression<Func<StockItem, bool>> filter = x => x.ProductId == stockDecreaseDto.ProductId;

            if (!await _stockRepository.ExistAsync(filter)) return false;

            var stockItem = await _stockRepository.GetAsync(filter);

            if(stockItem.Stock < stockDecreaseDto.Quantity) return false;

            stockItem.Stock -= stockDecreaseDto.Quantity;

            stockItem.LastUpdate = DateTimeOffset.UtcNow;

            await _stockRepository.UpdateAsync(stockItem.Id, stockItem);

            return true;
        }
    }
}
