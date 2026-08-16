using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Enums;
using Zentin.Payment.Domain.Events;

namespace Zentin.Payment.Domain.Entities.Merchant
{
    public sealed class MchInfo : AggregateRoot
    {
        public string MchNo { get; private set; } = string.Empty;
        public string MchName { get; private set; } = string.Empty;
        public string MchShortName { get; private set; } = string.Empty;
        public MchType Type { get; private set; }
        public string? IsvNo { get; private set; }
        public string MchLevel { get; private set; } = "M0";
        public List<string>? RefundMode { get; private set; }
        public string? Sipw { get; private set; }
        public string? TopAgentNo { get; private set; }
        public string? AgentNo { get; private set; }
        public string ContactName { get; private set; } = string.Empty;
        public string ContactTel { get; private set; } = string.Empty;
        public string? ContactEmail { get; private set; }
        public MchState State { get; private set; }
        public string? Remark { get; private set; }
        public long? InitUserId { get; private set; }
        public long? CreatedUid { get; private set; }
        public string? CreatedBy { get; private set; }

        
        //navigation properties for the aggregate root
        public IReadOnlyCollection<MchApp> Apps => _apps.AsReadOnly();
        private readonly List<MchApp> _apps = [];

        public IReadOnlyCollection<MchStore> Stores => _stores.AsReadOnly();
        private readonly List<MchStore> _stores = [];

        public IReadOnlyCollection<MbrRechargeRule> RechargeRules => _rechargeRules.AsReadOnly();
        private readonly List<MbrRechargeRule> _rechargeRules = [];

        private MchInfo() { }

  
        public static MchInfo Create(
            string mchNo,
            string mchName,
            string mchShortName,
            MchType type,
            string contactName,
            string contactTel,
            string? agentNo = null,
            string? isvNo = null,
            long? createdUid = null,
            string? createdBy = null)
        {
            if (string.IsNullOrWhiteSpace(mchNo))
                throw new ArgumentException("Merchant No is required", nameof(mchNo));
            if (string.IsNullOrWhiteSpace(mchName))
                throw new ArgumentException("Merchant Name is required", nameof(mchName));
            if (string.IsNullOrWhiteSpace(contactTel))
                throw new ArgumentException("Contact Phone is required", nameof(contactTel));

            var mch = new MchInfo
            {
                MchNo = mchNo,
                MchName = mchName,
                MchShortName = mchShortName,
                Type = type,
                ContactName = contactName,
                ContactTel = contactTel,
                AgentNo = agentNo,
                IsvNo = isvNo,
                CreatedUid = createdUid,
                CreatedBy = createdBy,
                MchLevel = "M0",
                State = MchState.Normal,
                RefundMode = ["api"]
            };

            mch.AddDomainEvent(new MchInfoCreatedDomainEvent(mchNo, mchName, contactTel, DateTime.UtcNow));
            return mch;
        }

        
        public void Disable(string? reason = null)
        {
            if (State == MchState.Disabled)
                throw new InvalidOperationException("Merchant is already disabled");

            var oldState = State;
            State = MchState.Disabled;
            Remark = reason;
            AddDomainEvent(new MchInfoStatusChangedDomainEvent(MchNo, (int)oldState, (int)State, DateTime.UtcNow));
        }

        
        public void Enable()
        {
            if (State != MchState.Disabled)
                throw new InvalidOperationException("only disabled merchants can be enabled");

            var oldState = State;
            State = MchState.Normal;
            AddDomainEvent(new MchInfoStatusChangedDomainEvent(MchNo, (int)oldState, (int)State, DateTime.UtcNow));
        }

        // 聚合内操作：添加应用
        public void AddApp(MchApp app)
        {
            if (State != MchState.Normal)
                throw new InvalidOperationException("only normal state merchants can add applications");
            if (_apps.Count >= 20)
                throw new InvalidOperationException("merchant application count has reached the limit");

            _apps.Add(app);
        }
    }
}
