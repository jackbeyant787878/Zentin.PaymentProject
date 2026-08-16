using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Enums;
using Zentin.Payment.Domain.Events;

namespace Zentin.Payment.Domain.Entities.Agent
{
    public sealed class AgentInfo : AggregateRoot
    {
        public string AgentNo { get; private set; } = string.Empty;
        public string AgentName { get; private set; } = string.Empty;
        public string AgentShortName { get; private set; } = string.Empty;
        public AgentType AgentType { get; private set; }
        public int Level { get; private set; }
        public string? Pid { get; private set; }
        public string? IsvNo { get; private set; }
        public string ContactName { get; private set; } = string.Empty;
        public string ContactTel { get; private set; } = string.Empty;
        public string? ContactEmail { get; private set; }
        public bool AddAgentFlag { get; private set; }
        public string? Sipw { get; private set; }
        public AgentState State { get; private set; }
        public string? Remark { get; private set; }
        public long? InitUserId { get; private set; }

        
        /// <summary>
        /// Settlement account type
        /// </summary>
        public SettAccountType? SettAccountType { get; private set; }
        public string? SettAccountTelphone { get; private set; }
        public string? SettAccountName { get; private set; }
        public string? SettAccountNo { get; private set; }
        public string? SettAccountBank { get; private set; }
        public string? SettAccountSubBank { get; private set; }

        
        /// <summary>
        /// Cashout fee rule type
        /// </summary>
        public CashoutFeeRuleType CashoutFeeRuleType { get; private set; }
        public string? CashoutFeeRule { get; private set; }

        
        /// <summary>
        /// Business license photo
        /// </summary>
        public string? LicenseImg { get; private set; }
        public string? PermitImg { get; private set; }
        public string? Idcard1Img { get; private set; }
        public string? Idcard2Img { get; private set; }
        public string? IdcardInHandImg { get; private set; }
        public string? BankCardImg { get; private set; }

        
        /// <summary>
        /// Available amount
        /// </summary>
        public long UnAmount { get; private set; }
        public long BalanceAmount { get; private set; }
        public long AuditProfitAmount { get; private set; }
        public long FreezeAmount { get; private set; }

        public long? CreatedUid { get; private set; }
        public string? CreatedBy { get; private set; }

       
        public AgentInfo? Parent { get; private set; }
        public ICollection<AgentInfo> Children { get; private set; } = new List<AgentInfo>();

        private AgentInfo() { }

      
        public static AgentInfo Create(
            string agentNo,
            string agentName,
            string agentShortName,
            AgentType agentType,
            int level,
            string contactName,
            string contactTel,
            string? pid = null,
            string? isvNo = null,
            long? createdUid = null,
            string? createdBy = null)
        {
            var agent = new AgentInfo
            {
                AgentNo = agentNo,
                AgentName = agentName,
                AgentShortName = agentShortName,
                AgentType = agentType,
                Level = level,
                Pid = pid,
                IsvNo = isvNo,
                ContactName = contactName,
                ContactTel = contactTel,
                State = AgentState.Normal,
                CashoutFeeRuleType = CashoutFeeRuleType.SystemDefault,
                CreatedUid = createdUid,
                CreatedBy = createdBy
            };

            agent.AddDomainEvent(new AgentInfoCreatedDomainEvent(agentNo, agentName, contactTel,DateTime.UtcNow));
            return agent;
        }

        
        /// <summary>
        /// Business behavior: disable the agent.
        /// </summary>
        /// <param name="reason"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void Disable(string? reason = null)
        {
            if (State == AgentState.Disabled)
                throw new InvalidOperationException("Agent was disabled");

            var old = State;
            State = AgentState.Disabled;
            Remark = reason;
            AddDomainEvent(new AgentInfoStatusChangedDomainEvent(AgentNo, (int)old, (int)State, DateTime.UtcNow));
        }

       
        /// <summary>
        /// business behavior: enable the agent. 
        /// Only disabled agents can be enabled. If the agent is not in a disabled state, an InvalidOperationException will be thrown.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void Enable()
        {
            if (State != AgentState.Disabled)
                throw new InvalidOperationException("Only disabled agents can be enabled");

            var old = State;
            State = AgentState.Normal;
            AddDomainEvent(new AgentInfoStatusChangedDomainEvent(AgentNo, (int)old, (int)State, DateTime.UtcNow));
        }

        
        /// <summary>
        /// Business behavior: change the balance of the agent's account.
        /// </summary>
        /// <param name="changeAmount"></param>
        /// <param name="bizType"></param>
        /// <param name="remark"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void ChangeBalance(long changeAmount, AccountBillBizType bizType, string? remark = null)
        {
            if (State != AgentState.Normal)
                throw new InvalidOperationException("Agent status is abnormal");

            var after = BalanceAmount + changeAmount;
            if (after < 0)
                throw new InvalidOperationException("Account balance is insufficient");

            BalanceAmount = after;
            AddDomainEvent(new AccountBalanceChangedDomainEvent(AgentNo, "AGENT", changeAmount, bizType, DateTime.UtcNow));
        }
    }
}
