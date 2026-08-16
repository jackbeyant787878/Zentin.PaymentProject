using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Zentin.Payment.Domain.Abstractions
{
    /// <summary>
    /// Generic repository base interface, only defines common data operation capabilities
    /// </summary>
    /// <typeparam name="TEntity">entity type</typeparam>
    /// <typeparam name="TKey">key type</typeparam>
    public interface IRepository<TEntity, in TKey> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);

        IQueryable<TEntity> GetQueryable();

        IQueryable<TEntity> GetQueryableNoTracking();

        void Add(TEntity entity);

        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        void AddRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity);

        void Remove(TEntity entity);

        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
