using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Infrastructure.Data;

namespace Zentin.Payment.Infrastructure.UnitOfWork
{
    /// <summary>
    /// unit of work implementation, unifying transactions, commits, and domain event publishing
    /// </summary>
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly PaymentDbContext _dbContext;
        private readonly IPublisher _publisher;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(PaymentDbContext dbContext, IPublisher publisher)
        {
            _dbContext = dbContext;
            _publisher = publisher;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // 1. first publish domain events (can be placed after commit based on requirements)
            await PublishDomainEventsAsync(cancellationToken);

            // 2. save changes to the database
            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return result;
        }

        public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            return _transaction.GetDbTransaction();
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null) return;
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null) return;
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        /// <summary>
        /// auto collect and publish domain events from aggregate roots
        /// </summary>
        private async Task PublishDomainEventsAsync(CancellationToken cancellationToken)
        {
            var domainEntities = _dbContext.ChangeTracker.Entries<AggregateRoot>().Where(e => e.Entity.DomainEvents.Any()).ToList();

            var domainEvents = domainEntities
                .SelectMany(e => e.Entity.DomainEvents)
                .ToList();

            // clear events to avoid duplicate publishing
            domainEntities.ForEach(e => e.Entity.ClearDomainEvents());

            foreach (var @event in domainEvents)
            {
                await _publisher.Publish(@event, cancellationToken);
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _dbContext.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction != null) await _transaction.DisposeAsync();
            await _dbContext.DisposeAsync();
        }

        public async Task<bool> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
