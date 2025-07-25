using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace App.Repositories
{
    public class GenericRepository<TEntity, TEntityId>: IGenericRepository<TEntity, TEntityId> 
        where TEntity : BaseEntity<TEntityId>
        where TEntityId : struct
    {
        protected readonly AppDbContext Context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            Context = context;
            _dbSet = Context.Set<TEntity>();
        }

        public async ValueTask AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<bool> AnyAsync(TEntityId id)
        {
            return await _dbSet.AnyAsync(x => x.Id.Equals(id));
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public IQueryable<TEntity> GetAll() => _dbSet.AsQueryable().AsNoTracking();

        public async ValueTask<TEntity?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
            => _dbSet.Where(predicate).AsQueryable().AsNoTracking();
    }
}
