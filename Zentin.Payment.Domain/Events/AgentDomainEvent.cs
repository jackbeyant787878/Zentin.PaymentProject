using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Domain.Events
{

    /// <summary>
    /// Agent creation successful event
    /// </summary>
    public sealed record AgentInfoCreatedDomainEvent(string AgentNo, string AgentName, string ContactTel,DateTime OccurredOnUtc) : IDomainEvent;

    /// <summary>
    /// Agent status change event
    /// </summary>
    public sealed record AgentInfoStatusChangedDomainEvent(string AgentNo, int OldState, int NewState, DateTime OccurredOnUtc) : IDomainEvent;

    /// <summary>
    /// Service provider creation success event
    /// </summary>
    public sealed record IsvInfoCreatedDomainEvent(string IsvNo, string IsvName, DateTime OccurredOnUtc) : IDomainEvent;

    /// <summary>
    /// Account balance change event
    /// </summary>
    public sealed record AccountBalanceChangedDomainEvent(string InfoId, string InfoType, long ChangeAmount, AccountBillBizType BizType, DateTime OccurredOnUtc) : IDomainEvent;
}
