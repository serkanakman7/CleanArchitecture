using System.Linq.Expressions;
using App.Domain.Entities.Common;

namespace App.Application.Contracts.Persistence
{
    public interface IGenericRepository<TEntity, TEntityId>
        where TEntity : BaseEntity<TEntityId>
        where TEntityId : struct
    {
        Task<List<TEntity>> GetAllAsync();
        Task<List<TEntity>> GetAllPagedAsync(int pageNumber, int pageSize);
        Task<List<TEntity>> Where(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyAsync(TEntityId id);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        ValueTask<TEntity?> GetByIdAsync(int id);
        ValueTask AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
