using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;

namespace Zentin.Payment.Domain.Events
{
    // <summary>
    /// Merchant creation success domain event
    /// </summary>


    public sealed record MchInfoCreatedDomainEvent(string MchNo, string MchName, string ContactTel, DateTime OccurredOnUtc ) : IDomainEvent;

    
   
    /// <summary>
    /// Merchant status changed domain event (enable/disable)
    /// </summary>
    public sealed record MchInfoStatusChangedDomainEvent(string MchNo, int OldState, int NewState, DateTime OccurredOnUtc) : IDomainEvent;

  
    

    /// <summary>
    /// Merchant audit passed domain event
    /// </summary>
    public sealed record MchInfoAuditPassedDomainEvent(string MchNo, string MchName, DateTime OccurredOnUtc) : IDomainEvent;


    /// <summary>
    /// Merchant application submission domain event
    /// </summary>
    public sealed record MchApplySubmittedDomainEvent(string ApplyId, string MchNo, string IfCode, DateTime OccurredOnUtc) : IDomainEvent;

    /// <summary>
    /// Merchant application success domain event
    /// </summary>
    public sealed record MchApplySuccessDomainEvent(string ApplyId, string MchNo, string ChannelApplyNo, DateTime OccurredOnUtc) : IDomainEvent;

    /// <summary>
    /// Member creation success domain event
    /// </summary>
    public sealed record MbrInfoCreatedDomainEvent(string MbrNo, string MchNo, string MbrTel, DateTime OccurredOnUtc) : IDomainEvent;

    /// <summary>
    /// Member recharge success domain event
    /// </summary>
    public sealed record MbrRechargeSuccessDomainEvent(string RechargeOrderId, string MbrNo, long PayAmount, long EntryAmount, DateTime OccurredOnUtc) : IDomainEvent;


}
