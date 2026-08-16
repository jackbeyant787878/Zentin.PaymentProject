using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Common;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Domain.Entities.Pay
{
    public sealed class PayRateConfig : BaseEntity
    {
        public long PayRateConfigId { get; private set; }
        public string ConfigType { get; private set; } = string.Empty;
        public string InfoType { get; private set; } = string.Empty;
        public string InfoId { get; private set; } = string.Empty;
        public string IfCode { get; private set; } = string.Empty;
        public string WayCode { get; private set; } = string.Empty;
        public FeeType FeeType { get; private set; }
        public LevelMode? LevelMode { get; private set; }
        public decimal? FeeRate { get; private set; }
        public bool ApplymentSupport { get; private set; }
        public bool State { get; private set; }

        public ICollection<PayRateLevelConfig> LevelConfigs { get; private set; } = new List<PayRateLevelConfig>();

        public PayRateConfig()
        {
            PayRateConfigId = SnowflakeIdGenerator.GetSnowflakeId();
        }


        #region 工厂创建方法
        /// <summary>
        ///  create single rate configuration
        /// </summary>
        public static PayRateConfig CreateSingleRate(string configType, string infoType, string infoId, string ifCode, string wayCode,
            decimal feeRate, bool applySupport = true, bool state = true)
        {
            //business validation
            if (string.IsNullOrWhiteSpace(configType))
                throw new ArgumentException("Config Type is required", nameof(configType));
            if (string.IsNullOrWhiteSpace(infoType))
                throw new ArgumentException("Info Type is required", nameof(infoType));
            if (string.IsNullOrWhiteSpace(infoId))
                throw new ArgumentException("Info ID is required", nameof(infoId));
            if (string.IsNullOrWhiteSpace(ifCode))
                throw new ArgumentException("If Code is required", nameof(ifCode));
            if (string.IsNullOrWhiteSpace(wayCode))
                throw new ArgumentException("Way Code is required", nameof(wayCode));
            if (feeRate < 0)
                throw new ArgumentException("Fee Rate cannot be negative", nameof(feeRate));

            var entity = new PayRateConfig
            {
                ConfigType = configType,
                InfoType = infoType,
                InfoId = infoId,
                IfCode = ifCode,
                WayCode = wayCode,
                FeeType = FeeType.Single,
                FeeRate = feeRate,
                LevelMode = null,
                ApplymentSupport = applySupport,
                State = state
            };
            return entity;
        }

        /// <summary>
        ///create level rate configuration
        /// </summary>
        public static PayRateConfig CreateLevelRate(string configType, string infoType,
            string infoId, string ifCode, string wayCode, bool applySupport = true, bool state = true)
        {
            if (string.IsNullOrWhiteSpace(configType) || string.IsNullOrWhiteSpace(infoType) || string.IsNullOrWhiteSpace(infoId) || string.IsNullOrWhiteSpace(ifCode) || string.IsNullOrWhiteSpace(wayCode))
                throw new ArgumentException("key parameters cannot be empty");

            var entity = new PayRateConfig
            {
                ConfigType = configType,
                InfoType = infoType,
                InfoId = infoId,
                IfCode = ifCode,
                WayCode = wayCode,
                FeeType = FeeType.Level,
                FeeRate = null,
                ApplymentSupport = applySupport,
                State = state
            };
            return entity;
        }
        #endregion

        #region business methods
        /// <summary>
        /// update single fee rate
        /// </summary>
        public void UpdateSingleFee(decimal newRate)
        {
            if (FeeType != FeeType.Single)
                throw new InvalidOperationException("current is level rate, cannot update single fee rate");
            if (newRate < 0)
                throw new ArgumentOutOfRangeException(nameof(newRate), "fee rate cannot be negative");

            FeeRate = newRate;
        }

        /// <summary>
        /// update level mode
        /// </summary>
        public void UpdateLevelMode(LevelMode mode)
        {
            if (FeeType != FeeType.Level)
                throw new InvalidOperationException("only level rates can modify level mode");
            LevelMode = mode;
        }



        /// <summary>
        /// switch applyment support
        /// </summary>
        public void SwitchApplySupport(bool enable)
        {
            ApplymentSupport = enable;
        }

        /// <summary>
        /// enable the configuration
        /// </summary>
        public void Enable()
        {
            State = true;
        }

        /// <summary>
        /// disable the configuration
        /// </summary>
        public void Disable()
        {
            State = false;
        }
        #endregion

    }
}
