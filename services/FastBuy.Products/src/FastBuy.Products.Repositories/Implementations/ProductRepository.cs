using FastBuy.Products.Entities;
using FastBuy.Products.Repositories.Abstractions;
using MongoDB.Driver;

namespace FastBuy.Products.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoDatabase _mongoDataBase;
        private readonly IMongoCollection<Product> _dbCollection;
        private readonly FilterDefinitionBuilder<Product> _filterDefinitionBuilder = Builders<Product>.Filter;
        private const string collectionName = "products";
        

        public ProductRepository(IMongoDatabase mongoDataBase)
        {
            _mongoDataBase = mongoDataBase;
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            FilterDefinition<Product> filter = _filterDefinitionBuilder.Eq(p => p.Id, id);

            return await _dbCollection.Find(filter).FirstOrDefaultAsync()
                ?? throw new KeyNotFoundException($"The resource with id {id} does not exist");
        }

        public async Task<IReadOnlyCollection<Product>> GetAllAsync()
            => await _dbCollection.Find(_filterDefinitionBuilder.Empty).ToListAsync();

        public async Task<Product> CreateAsync(Product product)
        {
            await _dbCollection.InsertOneAsync(product);

            return product;
        }

        public async Task UpdateAsync(Guid id, Product product)
        {
            var filter = _filterDefinitionBuilder.Eq(p => p.Id, id);

            var update = Builders<Product>.Update.Set(p => p.Name, product.Name)
                                                 .Set(p => p.Description, product.Description)
                                                 .Set(p => p.Price, product.Price);

            var result = await _dbCollection.FindOneAndUpdateAsync(filter, update)
                ?? throw new KeyNotFoundException($"The resource with id {id} does not exist");
                
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = _filterDefinitionBuilder.Eq(p => p.Id, id);

            var result = await _dbCollection.FindOneAndDeleteAsync(filter)
                ?? throw new KeyNotFoundException($"The resource with id {id} does not exist");                
        }
    }
}