using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Infrastructure.Data;
namespace Zentin.Payment.Infrastructure.Repositories
{
    /// <summary>
    /// Generic repository base class implementation
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TKey">Primary key type</typeparam>
    public abstract class RepositoryBase<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        protected readonly PaymentDbContext DbContext;
        protected readonly DbSet<TEntity> DbSet;

        protected RepositoryBase(PaymentDbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = dbContext.Set<TEntity>();
        }

        public virtual async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await DbSet.FindAsync(new object?[] { id! }, cancellationToken);
        }

        public virtual async Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public virtual async Task<bool> AnyAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await DbSet.AnyAsync(predicate, cancellationToken);
        }

        public virtual async Task<List<TEntity>> GetListAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            var query = DbSet.AsQueryable();
            if (predicate != null) query = query.Where(predicate);
            return await query.ToListAsync(cancellationToken);
        }

        public virtual void Add(TEntity entity) => DbSet.Add(entity);

        public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await DbSet.AddAsync(entity, cancellationToken);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities) => DbSet.AddRange(entities);

        public virtual void Update(TEntity entity) => DbSet.Update(entity);

        public virtual void Remove(TEntity entity) => DbSet.Remove(entity);

        public virtual void RemoveRange(IEnumerable<TEntity> entities) => DbSet.RemoveRange(entities);
    }
}
