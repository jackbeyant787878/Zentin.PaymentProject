using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Enums;
using Zentin.Payment.Domain.Events;

namespace Zentin.Payment.Domain.Entities.Isv
{
    public sealed class IsvInfo : AggregateRoot
    {
        public string IsvNo { get; private set; } = string.Empty;
        public string IsvName { get; private set; } = string.Empty;
        public string IsvShortName { get; private set; } = string.Empty;
        public string? ContactName { get; private set; }
        public string? ContactTel { get; private set; }
        public string? ContactEmail { get; private set; }
        public IsvState State { get; private set; }
        public string? Remark { get; private set; }
        public long? CreatedUid { get; private set; }
        public string? CreatedBy { get; private set; }

        private IsvInfo() { }

        public static IsvInfo Create(
            string isvNo,
            string isvName,
            string isvShortName,
            string? contactName = null,
            string? contactTel = null,
            long? createdUid = null,
            string? createdBy = null)
        {
            var isv = new IsvInfo
            {
                IsvNo = isvNo,
                IsvName = isvName,
                IsvShortName = isvShortName,
                ContactName = contactName,
                ContactTel = contactTel,
                State = IsvState.Normal,
                CreatedUid = createdUid,
                CreatedBy = createdBy
            };

            isv.AddDomainEvent(new IsvInfoCreatedDomainEvent(isvNo, isvName,DateTime.UtcNow));
            return isv;
        }

        public void Disable() => State = IsvState.Disabled;
        public void Enable() => State = IsvState.Normal;
    }
}
