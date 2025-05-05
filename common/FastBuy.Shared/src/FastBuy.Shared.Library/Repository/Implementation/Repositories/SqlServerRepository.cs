using FastBuy.Shared.Library.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FastBuy.Shared.Library.Repository.Implementation.Repositories
{
    public class SqlServerRepository<T> : IRepository<T> where T : class, IBaseEntity
    {
        private readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public SqlServerRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id)
                ?? throw new KeyNotFoundException($"The entity with ID {id} does not exist");
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.Where(filter).FirstOrDefaultAsync()
                ?? throw new KeyNotFoundException($"The entity does not exist"); ;
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            var query = _dbSet.AsQueryable().AsNoTracking();

            if(filter is not null) 
                query = query.Where(filter);

            return await query.ToListAsync();
        }

        public async Task CreateAsync(T entity)
        {
            await _context.AddAsync(entity);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guid id, T entity)
        {
            var existingEntity = await _dbSet.FindAsync(id)
                ?? throw new KeyNotFoundException($"The entity with ID {id} does not exist");

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var existingEntity = await _dbSet.FindAsync(id)
                ?? throw new KeyNotFoundException($"The entity with ID {id} does not exist");

            _context.Remove(existingEntity);

            await _context.SaveChangesAsync();

        }

        public Task<bool> ExistAsync(Expression<Func<T, bool>> filter)
        {
            return _dbSet.AnyAsync(filter);
        }
    }
}
