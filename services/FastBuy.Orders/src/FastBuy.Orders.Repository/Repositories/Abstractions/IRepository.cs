using FastBuy.Orders.Entities;
using System.Linq.Expressions;

namespace FastBuy.Orders.Repository.Repositories.Abstractions
{
    public interface IRepository<T> where T : class, IBaseEntity
    {
        Task<T?> GetAsync(Expression<Func<T, bool>> filter);
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
        Task CreateAsync(T entity);
        Task UpdateAsync(Guid id, T entity);
        Task DeleteAsync(Guid id);
    }
}
