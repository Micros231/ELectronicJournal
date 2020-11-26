using ElectronicJournal.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ElectronicJournal.EntityFrameworkCore.Data.Repositories
{
    public class EfCoreRepositoryBase<TDbContext, TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>, IRepository
        where TDbContext : DbContext
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected readonly TDbContext _dbContext;
        public EfCoreRepositoryBase(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<int> CountAsync()
        {
            return await _dbContext.Set<TEntity>().CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbContext.Set<TEntity>().CountAsync(predicate);
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TPrimaryKey id)
        {
            var entity =  await _dbContext.Set<TEntity>().FindAsync(id);
            if (entity != null)
            {
                _dbContext.Set<TEntity>().Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
            
        }

        public async Task<TEntity> FirstOrDefaultAsync()
        {
            return await _dbContext.Set<TEntity>().FirstOrDefaultAsync();
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().AsQueryable();
        }

        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var dbSet = _dbContext.Set<TEntity>();
            if (propertySelectors.Length == 0)
            {
                return dbSet.AsQueryable();
            }
           
            IQueryable<TEntity> query = _dbContext.Set<TEntity>().AsQueryable();
            foreach (var propertySelector in propertySelectors)
            {
                if (propertySelector != null)
                {
                    query = query.Include(propertySelector);
                }
            }
            return query;
        }

        public IQueryable<TEntity> GetAllIncludingAndThenIncluding<TPreviousProperty, TProperty>(Expression<Func<TEntity, IEnumerable<TPreviousProperty>>> includingProperty, Expression<Func<TPreviousProperty, TProperty>> thenIncludingProperty)
        {
            var dbSet = _dbContext.Set<TEntity>();
            if (includingProperty != null && thenIncludingProperty != null)
            {
                var query = dbSet.Include(includingProperty).ThenInclude(thenIncludingProperty);
                return query;
            }
            return dbSet.AsQueryable();
        }

        public virtual async Task<List<TEntity>> GetAllListAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            var addedEntity = await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return addedEntity.Entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return entity;
        }
    }
}
