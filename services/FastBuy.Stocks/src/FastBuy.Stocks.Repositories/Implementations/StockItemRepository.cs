using FastBuy.Stocks.Contracts;
using FastBuy.Stocks.Entities;
using FastBuy.Stocks.Entities.Configuration;
using FastBuy.Stocks.Repositories.Abstractions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FastBuy.Stocks.Repositories.Implementations
{
    public class StockItemRepository : IStockItemRepository
    {
        private readonly IMongoCollection<StockItem> _dbCollection;
        private readonly FilterDefinitionBuilder<StockItem> _filterDefinitionBuilder = Builders<StockItem>.Filter;

        public StockItemRepository(IMongoDatabase database, IOptions<ServiceSettings> settings)
        {
            _dbCollection = database.GetCollection<StockItem>(settings.Value.ServiceName);
        }


        public async Task<StockItem> GetByProductIdAsync(Guid ProductId)
        {    
            FilterDefinition<StockItem> filter = _filterDefinitionBuilder.Eq(s => s.ProductId, ProductId);

            return await _dbCollection.Find(filter).FirstOrDefaultAsync();
        }


        public async Task<IReadOnlyCollection<StockItem>> GetAllAsync()
        {
            return await _dbCollection.Find(_filterDefinitionBuilder.Empty).ToListAsync();
        }


        public async Task<StockItem> CreateAsync(StockItem stockItem)
        {
            await _dbCollection.InsertOneAsync(stockItem);

            return stockItem;
        }


        public async Task UpdateAsync(Guid id, StockItem stockItem)
        {
            var filter = _filterDefinitionBuilder.Eq(p => p.Id, id);

            var result = await _dbCollection.FindOneAndReplaceAsync(filter, stockItem)
                ?? throw new KeyNotFoundException($"The resource with id {id} does not exist");
        }

        public async Task UpdateStockAsync(StockItem stockItem)
        {
            var filter = _filterDefinitionBuilder.Eq(p => p.ProductId, stockItem.ProductId);

            var update = Builders<StockItem>.Update.Set(s => s.Stock, stockItem.Stock)
                                                   .Set(s => s.LastUpdate, stockItem.LastUpdate);
            
            var result = await _dbCollection.FindOneAndUpdateAsync(filter, update)
                ?? throw new KeyNotFoundException($"The resource with id {stockItem.ProductId} does not exist");
        }
    }
}
