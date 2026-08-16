using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Domain.Entities.Member
{
    public sealed class MbrBill : BaseEntity
    {
        public string MbrBillId { get; private set; } = string.Empty;
        public string MbrNo { get; private set; } = string.Empty;
        public string MbrName { get; private set; } = string.Empty;
        public string MchNo { get; private set; } = string.Empty;
        public string? MbrTel { get; private set; }
        public long BeforeBalance { get; private set; }
        public long ChangeAmount { get; private set; }
        public long AfterBalance { get; private set; }
        public MbrBillBizType BizType { get; private set; }
        public string? Remark { get; private set; }

       
        public MbrInfo MbrInfo { get; private set; } = null!;

        private MbrBill() { }

        internal static MbrBill Create(
            string mbrBillId,
            string mbrNo,
            string mbrName,
            string mchNo,
            string? mbrTel,
            long beforeBalance,
            long changeAmount,
            long afterBalance,
            MbrBillBizType bizType,
            string? remark)
        {
            return new MbrBill
            {
                MbrBillId = mbrBillId,
                MbrNo = mbrNo,
                MbrName = mbrName,
                MchNo = mchNo,
                MbrTel = mbrTel,
                BeforeBalance = beforeBalance,
                ChangeAmount = changeAmount,
                AfterBalance = afterBalance,
                BizType = bizType,
                Remark = remark
            };
        }
    }
}
