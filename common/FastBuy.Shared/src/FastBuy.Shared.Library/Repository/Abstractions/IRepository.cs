using System.Linq.Expressions;

namespace FastBuy.Shared.Library.Repository.Abstractions
{
    public interface IRepository<T> where T : class, IBaseEntity
    {        
        Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter);
        Task<T> GetByIdAsync(Guid id);
        Task CreateAsync(T entity);
        Task UpdateAsync(Guid id, T entity);
        Task DeleteAsync(Guid id);
        Task<bool> ExistAsync(Expression<Func<T, bool>> filter);
    }
}
