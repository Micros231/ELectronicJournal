using ElectronicJournal.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ElectronicJournal.EntityFrameworkCore.Data.Repositories
{
    public interface IRepository
    {

    }
    public interface IRepository<TEntity> : IRepository<TEntity, int>
        where TEntity : class, IEntity<int>
    {

    }
    public interface IRepository<TEntity, TPrimaryKey> : IRepository
        where TEntity : class, IEntity<TPrimaryKey>
    {
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync();
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> InsertAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(TPrimaryKey id);
        Task<TEntity> GetAsync(TPrimaryKey id);
        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors);
        IQueryable<TEntity> GetAllIncludingAndThenIncluding<TPreviousProperty, TProperty>(
            Expression<Func<TEntity, IEnumerable<TPreviousProperty>>> includingProperty, 
            Expression<Func<TPreviousProperty, TProperty>> thenIncludingProperty);
        IQueryable<TEntity> GetAll();
        Task<List<TEntity>> GetAllListAsync();
        Task<TEntity> UpdateAsync(TEntity entity);

    }
    
}
