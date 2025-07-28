using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using App.Application.Contracts.Persistence;
using App.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence
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

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet.AsQueryable().AsNoTracking().ToListAsync();
        }

        public async Task<List<TEntity>> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            return await _dbSet.Skip((pageNumber -1)*pageSize).Take(pageSize).ToListAsync();
        }

        public async ValueTask<TEntity?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        async Task<List<TEntity>> IGenericRepository<TEntity, TEntityId>.Where(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).AsQueryable().AsNoTracking().ToListAsync();
        }
    }
}
