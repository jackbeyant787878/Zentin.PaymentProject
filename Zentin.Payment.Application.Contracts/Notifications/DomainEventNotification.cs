using MediatR;
using Zentin.Payment.Domain.Abstractions;

namespace Zentin.Payment.Application.Contracts.Notifications
{
    // 领域事件(IDomainEvent)本身不依赖 MediatR；此适配器让基础设施层经 MediatR 发布、应用层经 MediatR 订阅
    public sealed class DomainEventNotification : INotification
    {
        public IDomainEvent DomainEvent { get; }

        public DomainEventNotification(IDomainEvent domainEvent) => DomainEvent = domainEvent;
    }
}
