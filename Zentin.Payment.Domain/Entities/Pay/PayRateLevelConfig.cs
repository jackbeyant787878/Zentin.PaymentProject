using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Common;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Domain.Entities.Pay
{
    public sealed class PayRateLevelConfig : BaseEntity
    {
        public long PayRateLevelConfigId { get; private set; }
        public long RateConfigId { get; private set; }
        public BankCardType? BankCardType { get; private set; }
        public int MinAmount { get; private set; }
        public int MaxAmount { get; private set; }
        public int MinFee { get; private set; }
        public int MaxFee { get; private set; }
        public decimal FeeRate { get; private set; }
        public bool State { get; private set; }

        public PayRateConfig RateConfig { get; private set; } = null!;

        public PayRateLevelConfig()
        {
            PayRateLevelConfigId = SnowflakeIdGenerator.GetSnowflakeId();
        }


        #region 工厂方法
        /// <summary>
        /// 创建阶梯费率配置
        /// </summary>
        public static PayRateLevelConfig Create(long rateConfigId, decimal feeRate, int minAmount = 0, int maxAmount = 0,
            int minFee = 0,int maxFee = 0,BankCardType? bankCardType = null,bool state = true)
        {
            if (feeRate < 0)
                throw new ArgumentException("费率不能为负数", nameof(feeRate));
            if (minAmount < 0)
                throw new ArgumentException("最小金额不能为负数", nameof(minAmount));
            if (maxAmount < 0)
                throw new ArgumentException("最大金额不能为负数", nameof(maxAmount));
            if (maxAmount > 0 && minAmount >= maxAmount)
                throw new ArgumentException("最小金额必须小于最大金额");

            return new PayRateLevelConfig
            {
                RateConfigId = rateConfigId,
                BankCardType = bankCardType,
                MinAmount = minAmount,
                MaxAmount = maxAmount,
                MinFee = minFee,
                MaxFee = maxFee,
                FeeRate = feeRate,
                State = state
            };
        }
        #endregion

        #region  business methods
        /// <summary>
        /// update the level configuration
        /// </summary>
        public void Update(
            decimal feeRate,
            int minAmount,
            int maxAmount,
            int minFee,
            int maxFee,
            BankCardType? bankCardType = null)
        {
            if (feeRate < 0)
                throw new ArgumentException("Fee rate cannot be negative", nameof(feeRate));

            FeeRate = feeRate;
            MinAmount = minAmount;
            MaxAmount = maxAmount;
            MinFee = minFee;
            MaxFee = maxFee;
            BankCardType = bankCardType;
        }

        public void Enable() => State = true;
        public void Disable() => State = false;
        #endregion
    }
}
