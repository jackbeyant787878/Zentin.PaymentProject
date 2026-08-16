using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;

namespace Zentin.Payment.Domain.Events
{
 
    public sealed record RefundSuccessDomainEvent(string refundOrderId,string payOrderId, long refundAmount, DateTime OccurredOnUtc) : IDomainEvent;
}
