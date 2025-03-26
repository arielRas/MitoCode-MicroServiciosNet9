using FastBuy.Stocks.Contracts;
using FastBuy.Stocks.Repositories.Abstractions;
using FastBuy.Stocks.Services.Abstractions;
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

        public Task<IEnumerable<StockResponseDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }


        public Task<StockResponseDto> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }


        public Task<StockResponseDto> GetByProductIdAsync(Guid productId)
        {
            throw new NotImplementedException();
        }


        public Task<bool> SetStockAsync(Guid productId, int stock)
        {
            throw new NotImplementedException();
        }


        public Task<bool> DecreaseStockAsync(StockDecreaseRequestDto stockDecreaseDto)
        {
            throw new NotImplementedException();
        }
    }
}
