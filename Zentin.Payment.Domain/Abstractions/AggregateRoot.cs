namespace Zentin.Payment.Domain.Abstractions
{
    public abstract class AggregateRoot : BaseEntity, IAggregateRoot
    {

        private readonly List<IDomainEvent> _domainEvents = [];
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        public void ClearDomainEvents() => _domainEvents.Clear();

    }

    // 领域事件是纯领域概念，不依赖 MediatR；由基础设施层经 DomainEventNotification 包装后发布
    public interface IDomainEvent
    {
        DateTime OccurredOnUtc { get; }


    }

}
