using DotNetCore.CAP;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using Zentin.Payment.Application.Contracts.IntegrationEvents;
using Zentin.Payment.Application.Contracts.Notifications;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Events;
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
        private readonly ICapPublisher _capPublisher;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(PaymentDbContext dbContext, IPublisher publisher, ICapPublisher capPublisher)
        {
            _dbContext = dbContext;
            _publisher = publisher;
            _capPublisher = capPublisher;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var domainEvents = CollectDomainEvents();
            if (domainEvents.Count == 0)
                return await _dbContext.SaveChangesAsync(cancellationToken);

            var result = 0;
            // EnableRetryOnFailure 的执行策略不允许裸用户事务，须整体包进策略以支持瞬时故障重试
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                // outbox 与业务写入同事务：要么都提交要么都回滚（ADR-0006）
                await using var transaction = _dbContext.Database.BeginTransaction(_capPublisher, autoCommit: false);
                result = await _dbContext.SaveChangesAsync(cancellationToken);

                foreach (var @event in domainEvents)
                {
                    var message = MapToIntegrationEvent(@event);
                    if (message == null) continue;

                    if (message.Value.Delay != null)
                        await _capPublisher.PublishDelayAsync(message.Value.Delay.Value, message.Value.Topic, message.Value.Payload, headers: new Dictionary<string, string>(), cancellationToken: cancellationToken);
                    else
                        await _capPublisher.PublishAsync(message.Value.Topic, message.Value.Payload, cancellationToken: cancellationToken);
                }

                await transaction.CommitAsync(cancellationToken);
            });

            // 进程内订阅者在提交之后派发，确保看到已提交状态
            foreach (var @event in domainEvents)
                await _publisher.Publish(new DomainEventNotification(@event), cancellationToken);

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

        private List<IDomainEvent> CollectDomainEvents()
        {
            var entries = _dbContext.ChangeTracker.Entries<AggregateRoot>()
                .Where(e => e.Entity.DomainEvents.Any()).ToList();

            var events = entries.SelectMany(e => e.Entity.DomainEvents).ToList();
            entries.ForEach(e => e.Entity.ClearDomainEvents());
            return events;
        }

        // 显式映射表，禁止反射扫描（铁律 5 精神）；新领域事件需要出站时在此登记
        private static (string Topic, object Payload, TimeSpan? Delay)? MapToIntegrationEvent(IDomainEvent @event) => @event switch
        {
            PayOrderPaidDomainEvent paid => (IntegrationEventTopics.PayOrderPaid,
                new PayOrderPaidIntegrationEvent(paid.PayOrderId, paid.MchNo, paid.Amount, paid.OccurredOnUtc), null),
            MchNotifyCreatedDomainEvent created => (IntegrationEventTopics.MchNotifyDispatch,
                new MchNotifyDispatchIntegrationEvent(created.NotifyId), null),
            MchNotifyAttemptedDomainEvent attempted => (IntegrationEventTopics.MchNotifyDispatch,
                new MchNotifyDispatchIntegrationEvent(attempted.NotifyId), TimeSpan.FromSeconds(attempted.DelaySeconds)),
            _ => null
        };

        public void Dispose()
        {
            _transaction?.Dispose();
            // DbContext 由 DI 容器（对象池）管理生命周期，此处不得手动释放
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction != null) await _transaction.DisposeAsync();
            // DbContext 由 DI 容器（对象池）管理生命周期，此处不得手动释放
        }

        public async Task<bool> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}