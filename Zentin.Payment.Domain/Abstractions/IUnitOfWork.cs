using System.Data;

namespace Zentin.Payment.Domain.Abstractions
{
    /// <summary>
    /// work unit interface, unified management of transactions and data submission
    /// </summary>
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        Task<bool> CommitAsync();
    }
}
