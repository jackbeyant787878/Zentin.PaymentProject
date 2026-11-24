using System;
using Zentin.Payment.Domain.Abstractions;

namespace Zentin.Payment.Domain.Events
{
    /// <summary>
    /// Raised when a refund order transitions to success; consumed by merchant notify and (later) profit settlement.
    /// </summary>
    public sealed record RefundSuccessDomainEvent(string RefundOrderId, string PayOrderId, long RefundAmount, DateTime OccurredOnUtc) : IDomainEvent;
}
