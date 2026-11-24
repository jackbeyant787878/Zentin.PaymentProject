using System;
using Zentin.Payment.Domain.Abstractions;

namespace Zentin.Payment.Domain.Events
{
    /// <summary>
    /// Raised when a merchant notify record is created; maps to the dispatch integration event.
    /// </summary>
    public sealed record MchNotifyCreatedDomainEvent(long NotifyId, DateTime OccurredOnUtc) : IDomainEvent;

    /// <summary>
    /// Raised when a notify attempt did not succeed and retries remain; maps to a delayed dispatch message.
    /// </summary>
    public sealed record MchNotifyAttemptedDomainEvent(long NotifyId, int DelaySeconds, DateTime OccurredOnUtc) : IDomainEvent;
}