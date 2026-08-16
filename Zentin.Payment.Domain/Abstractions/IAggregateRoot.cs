using System;
using System.Collections.Generic;
using System.Text;

namespace Zentin.Payment.Domain.Abstractions
{
    public interface IAggregateRoot
    {
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
        void ClearDomainEvents();

        void AddDomainEvent(IDomainEvent domainEvent);

    }
}
