using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;

namespace Zentin.Payment.Domain.Events
{
  
    public sealed record TransferSuccessDomainEvent(string transferId, string mchNo, long amount, DateTime OccurredOnUtc) : IDomainEvent;
}
