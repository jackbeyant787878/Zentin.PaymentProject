using Zentin.Payment.Common;
using Zentin.Payment.Domain.Abstractions;

namespace Zentin.Payment.Domain.Entities.Pay
{
    public sealed class PayInterfaceConfig : BaseEntity
    {
        public long PayInterfaceConfigId { get; private set; }
        public string InfoType { get; private set; } = string.Empty;
        public string InfoId { get; private set; } = string.Empty;
        public string IfCode { get; private set; } = string.Empty;
        public string IfParams { get; private set; } = string.Empty;
        public int SettHoldDay { get; private set; }
        public decimal? IfRate { get; private set; }
        public bool IsOpenApplyment { get; private set; }
        public bool IsOpenCashout { get; private set; }
        public bool IsOpenCheckBill { get; private set; }
        public string? IgnoreCheckBillMchNos { get; private set; }
        public string? Oauth2InfoId { get; private set; }
        public bool State { get; private set; }
        public string? Remark { get; private set; }
        public long? CreatedUid { get; private set; }
        public string? CreatedBy { get; private set; }
        public long? UpdatedUid { get; private set; }
        public string? UpdatedBy { get; private set; }

        public PayInterfaceConfig()
        {
            PayInterfaceConfigId = SnowflakeIdGenerator.GetSnowflakeId();
        }


        public static PayInterfaceConfig Create(string infoType, string infoId, string ifCode,
        string ifParams, int settHoldDay = 0, decimal? ifRate = null, long? createdUid = null, string? createdBy = null,
        string? remark = null, bool state = true)
        {
            if (string.IsNullOrWhiteSpace(infoType))
                throw new ArgumentException("Account Type cannot be null or empty", nameof(infoType));
            if (string.IsNullOrWhiteSpace(infoId))
                throw new ArgumentException("Account ID cannot be null or empty", nameof(infoId));
            if (string.IsNullOrWhiteSpace(ifCode))
                throw new ArgumentException("Interface Code cannot be null or empty", nameof(ifCode));
            if (string.IsNullOrWhiteSpace(ifParams))
                throw new ArgumentException("Interface Config Params cannot be null or empty", nameof(ifParams));
            if (settHoldDay < 0)
                throw new ArgumentException("Settlement Hold Day cannot be negative", nameof(settHoldDay));

            return new PayInterfaceConfig
            {
                InfoType = infoType,
                InfoId = infoId,
                IfCode = ifCode,
                IfParams = ifParams,
                SettHoldDay = settHoldDay,
                IfRate = ifRate,
                CreatedUid = createdUid,
                CreatedBy = createdBy,
                Remark = remark,
                State = state
            };
        }

        // ========== business methods ==========
        /// <summary>
        /// update configuration parameters
        /// </summary>
        public void UpdateParams(string ifParams, long? updatedUid = null, string? updatedBy = null)
        {
            if (string.IsNullOrWhiteSpace(ifParams))
                throw new ArgumentException("Interface Config Params cannot be null or empty", nameof(ifParams));

            IfParams = ifParams;
            UpdatedUid = updatedUid;
            UpdatedBy = updatedBy;
        }

        /// <summary>
        /// set settlement and rate
        /// </summary>
        public void SetSettAndRate(int settHoldDay, decimal ifRate)
        {
            if (settHoldDay < 0)
                throw new ArgumentException("Settlement Hold Day cannot be negative", nameof(settHoldDay));
            if (ifRate < 0)
                throw new ArgumentException("Interface Rate cannot be negative", nameof(ifRate));

            SettHoldDay = settHoldDay;
            IfRate = ifRate;
        }

        /// <summary>
        /// set feature switches
        /// </summary>
        public void SetSwitches(bool isOpenApplyment, bool isOpenCashout, bool isOpenCheckBill, string? ignoreCheckBillMchNos = null, string? oauth2InfoId = null)
        {
            IsOpenApplyment = isOpenApplyment;
            IsOpenCashout = isOpenCashout;
            IsOpenCheckBill = isOpenCheckBill;
            IgnoreCheckBillMchNos = ignoreCheckBillMchNos;
            Oauth2InfoId = oauth2InfoId;
        }

        public void Enable() => State = true;
        public void Disable() => State = false;
    }
}
