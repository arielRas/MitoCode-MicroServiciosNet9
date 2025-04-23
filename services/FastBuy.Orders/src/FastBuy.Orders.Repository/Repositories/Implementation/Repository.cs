using FastBuy.Orders.Entities;
using FastBuy.Orders.Repository.Database;
using FastBuy.Orders.Repository.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FastBuy.Orders.Repository.Repositories.Implementation
{
    public class Repository<T> : IRepository<T> where T : class, IBaseEntity
    {
        protected readonly OrdersDbContext _context;

        protected DbSet<T> dbSet;

        public Repository(OrdersDbContext context)
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
            return await dbSet.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
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
