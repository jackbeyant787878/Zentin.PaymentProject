using MediatR;

namespace Zentin.Payment.Domain.Abstractions
{
    public abstract class AggregateRoot : BaseEntity, IAggregateRoot
    {

        private readonly List<IDomainEvent> _domainEvents = [];
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        public void ClearDomainEvents() => _domainEvents.Clear();

    }


    public interface IDomainEvent : INotification
    {
        DateTime OccurredOnUtc { get; }


    }

}
