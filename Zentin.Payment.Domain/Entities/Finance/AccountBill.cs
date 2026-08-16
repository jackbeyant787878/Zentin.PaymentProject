using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Common;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Domain.Entities.Finance
{
    public sealed class AccountBill : BaseEntity
    {
        public long AccountId { get; private set; }
        public string BillId { get; private set; } = string.Empty;
        public string InfoId { get; private set; } = string.Empty;
        public string InfoName { get; private set; } = string.Empty;
        public string InfoType { get; private set; } = string.Empty;
        public long BeforeBalance { get; private set; }
        public long ChangeAmount { get; private set; }
        public long AfterBalance { get; private set; }
        public AccountBillBizType BizType { get; private set; }
        public AccountType AccountType { get; private set; }
        public RelaBizOrderType RelaBizOrderType { get; private set; }
        public string? RelaBizOrderId { get; private set; }
        public string? Remark { get; private set; }

        public AccountBill()
        {
            AccountId = SnowflakeIdGenerator.GetSnowflakeId();
        }

        public static AccountBill Create(
            string billId,
            string infoId,
            string infoName,
            string infoType,
            long beforeBalance,
            long changeAmount,
            long afterBalance,
            AccountBillBizType bizType,
            AccountType accountType,
            RelaBizOrderType relaBizOrderType,
            string? relaBizOrderId = null,
            string? remark = null)
        {
            return new AccountBill
            {
                BillId = billId,
                InfoId = infoId,
                InfoName = infoName,
                InfoType = infoType,
                BeforeBalance = beforeBalance,
                ChangeAmount = changeAmount,
                AfterBalance = afterBalance,
                BizType = bizType,
                AccountType = accountType,
                RelaBizOrderType = relaBizOrderType,
                RelaBizOrderId = relaBizOrderId,
                Remark = remark
            };
        }
    }
}
