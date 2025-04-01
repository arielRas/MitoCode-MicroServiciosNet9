using FastBuy.Shared.Library.Repository.Abstractions;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace FastBuy.Shared.Library.Repository.Implementation
{
    public class MongoDbRepository<T> : IRepository<T> where T : class, IBaseEntity
    {
        private readonly IMongoCollection<T> _collection;
        private readonly FilterDefinitionBuilder<T> _filterBuilder = Builders<T>.Filter;

        public MongoDbRepository(IMongoDatabase database, string collectionName)
        {
            _collection = database.GetCollection<T>(collectionName);
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var filter = _filterBuilder.Eq(x => x.Id, id);

            var entity = await _collection.Find(filter).FirstOrDefaultAsync()
                ?? throw new KeyNotFoundException($"The entity with ID {id} does not exist");

            return entity;
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            if (filter is null)
                return await _collection.Find(_filterBuilder.Empty).ToListAsync();

            return await _collection.Find(filter).ToListAsync();
        }

        public async Task CreateAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(Guid id, T entity)
        {
            var filter = _filterBuilder.Eq(x => x.Id, id);

            var result = await _collection.FindOneAndReplaceAsync(filter, entity)
                ?? throw new KeyNotFoundException($"The entity with ID {id} does not exist"); 
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = _filterBuilder.Eq(x => x.Id, id);

            var result = await _collection.FindOneAndDeleteAsync(filter)
                ?? throw new KeyNotFoundException($"The entity with ID {id} does not exist");
        }        
    }
}
