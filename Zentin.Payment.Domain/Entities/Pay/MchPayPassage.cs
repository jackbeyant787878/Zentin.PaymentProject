using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Common;
using Zentin.Payment.Domain.Abstractions;

namespace Zentin.Payment.Domain.Entities.Pay
{
    public sealed class MchPayPassage : BaseEntity
    {
        public long MchPayPassageId { get; private set; }
        public string MchNo { get; private set; } = string.Empty;
        public string AppId { get; private set; } = string.Empty;
        public string IfCode { get; private set; } = string.Empty;
        public string WayCode { get; private set; } = string.Empty;
        public decimal Rate { get; private set; }
        public string? RiskConfig { get; private set; }
        public bool State { get; private set; }

        public MchPayPassage()
        {
            MchPayPassageId = SnowflakeIdGenerator.GetSnowflakeId();
        }

        #region  factory methods
        /// <summary>
        /// create merchant payment passage
        /// </summary>
        public static MchPayPassage Create(string mchNo,string appId,string ifCode, string wayCode,decimal rate, string? riskConfig = null,bool state = true    )
        {
            if (string.IsNullOrWhiteSpace(mchNo))
                throw new ArgumentException("Merchant No is required", nameof(mchNo));
            if (string.IsNullOrWhiteSpace(appId))
                throw new ArgumentException("App ID is required", nameof(appId));
            if (string.IsNullOrWhiteSpace(ifCode))
                throw new ArgumentException("Interface Code is required", nameof(ifCode));
            if (string.IsNullOrWhiteSpace(wayCode))
                throw new ArgumentException("Payment Method Code is required", nameof(wayCode));
            if (rate < 0)
                throw new ArgumentException("Fee Rate cannot be negative", nameof(rate));

            return new MchPayPassage
            {
                MchNo = mchNo,
                AppId = appId,
                IfCode = ifCode,
                WayCode = wayCode,
                Rate = rate,
                RiskConfig = riskConfig,
                State = state
            };
        }
        #endregion

        #region 业务方法
        /// <summary>
        ///  update fee rate
        /// </summary>
        public void UpdateRate(decimal rate)
        {
            if (rate < 0)
                throw new ArgumentException("Fee Rate cannot be negative", nameof(rate));

            Rate = rate;
        }

        /// <summary>
        /// update risk configuration
        /// </summary>
        public void UpdateRiskConfig(string riskConfig)
        {
            RiskConfig = riskConfig;
        }

        public void Enable() => State = true;
        public void Disable() => State = false;
        #endregion

    }
}
