using Zentin.Payment.Common;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Domain.Entities.Merchant
{
    public sealed class MbrRechargeRule : BaseEntity
    {
        public long RuleId { get; private set; }
        public string MchNo { get; private set; } = string.Empty;
        public long RechargeAmount { get; private set; }
        public long GiveAmount { get; private set; }
        public long Sort { get; private set; }
        public string? Remark { get; private set; }
        public MbrState State { get; private set; }

        public MchInfo MchInfo { get; private set; } = null!;

        public MbrRechargeRule()
        {
            RuleId = SnowflakeIdGenerator.GetSnowflakeId();
        }
    }
}
