using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;

namespace Zentin.Payment.Domain.Events
{
    /// <summary>
    /// PayOrderPaidDomainEvent represents a domain event that is triggered when a payment order has been successfully paid. 
    /// </summary>
    public sealed record PayOrderPaidDomainEvent(string PayOrderId,string MchNo,long Amount,DateTime OccurredOnUtc) : IDomainEvent;
}
