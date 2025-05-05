using FastBuy.Payments.Api.Persistence.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FastBuy.Payments.Api.Persistence.Repository.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly PaymentsDbContext _context;

        protected DbSet<T> dbSet;

        public Repository(PaymentsDbContext context)
        {
            _context = context;
            dbSet = context.Set<T>();
        }


        public async Task<T?> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await dbSet.AsNoTracking().Where(filter).FirstOrDefaultAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet.AsQueryable();

            query = query.AsNoTracking();

            if (filter is not null)
                query = query.Where(filter);

            return await query.ToListAsync();
        }

        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Guid id, T entity)
        {
            var existingEntity = await dbSet.FindAsync(id)
                ?? throw new KeyNotFoundException($"The resource with id {id} does not exist");

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var existingEntity = await dbSet.FindAsync(id)
                ?? throw new KeyNotFoundException($"The resource with id {id} does not exist");

            _context.Remove(existingEntity);

            await _context.SaveChangesAsync();
        }
    }
}
