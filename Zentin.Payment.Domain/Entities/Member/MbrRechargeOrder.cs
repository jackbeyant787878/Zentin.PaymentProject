using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Enums;
using Zentin.Payment.Domain.Events;

namespace Zentin.Payment.Domain.Entities.Member
{
    public sealed class MbrRechargeOrder : AggregateRoot
    {
        public string RechargeOrderId { get; private set; } = string.Empty;
        public string MbrNo { get; private set; } = string.Empty;
        public string MbrName { get; private set; } = string.Empty;
        public string MchNo { get; private set; } = string.Empty;
        public string? MbrTel { get; private set; }
        public long PayOrderId { get; private set; }
        public string WayCode { get; private set; } = string.Empty;
        public string WayName { get; private set; } = string.Empty;
        public string WayType { get; private set; } = string.Empty;
        public long PayAmount { get; private set; }
        public long EntryAmount { get; private set; }
        public long GiveAmount { get; private set; }
        public long AfterBalance { get; private set; }
        public string? AvatarUrl { get; private set; }
        public string? Remark { get; private set; }
        public MbrRechargeState State { get; private set; }
        public string NotifyUrl { get; private set; } = string.Empty;
        public string? ReturnUrl { get; private set; }
        public DateTime? ExpiredTime { get; private set; }
        public DateTime? SuccessTime { get; private set; }

        private MbrRechargeOrder() { }

   
        public static MbrRechargeOrder Create(
            string rechargeOrderId,
            string mbrNo,
            string mbrName,
            string mchNo,
            string wayCode,
            string wayName,
            string wayType,
            long payAmount,
            long entryAmount,
            long giveAmount,
            string notifyUrl,
            DateTime? expiredTime = null,
            string? remark = null)
        {
            return new MbrRechargeOrder
            {
                RechargeOrderId = rechargeOrderId,
                MbrNo = mbrNo,
                MbrName = mbrName,
                MchNo = mchNo,
                WayCode = wayCode,
                WayName = wayName,
                WayType = wayType,
                PayAmount = payAmount,
                EntryAmount = entryAmount,
                GiveAmount = giveAmount,
                NotifyUrl = notifyUrl,
                ExpiredTime = expiredTime,
                Remark = remark,
                State = MbrRechargeState.Init
            };
        }

        
        public void MarkSuccess(long payOrderId, long afterBalance)
        {
            if (State is MbrRechargeState.Success or MbrRechargeState.Failed)
                throw new InvalidOperationException("current order state is already success or failed");

            State = MbrRechargeState.Success;
            PayOrderId = payOrderId;
            AfterBalance = afterBalance;
            SuccessTime = DateTime.UtcNow;

            AddDomainEvent(new MbrRechargeSuccessDomainEvent(RechargeOrderId, MbrNo, PayAmount, EntryAmount,DateTime.UtcNow));
        }

        
        public void MarkFail(string? errMsg = null)
        {
            if (State is MbrRechargeState.Success or MbrRechargeState.Failed)
                throw new InvalidOperationException("current order state is already success or failed");

            State = MbrRechargeState.Failed;
            Remark = errMsg;
        }
    }
}
