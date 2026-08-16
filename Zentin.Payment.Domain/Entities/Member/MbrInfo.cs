using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Enums;
using Zentin.Payment.Domain.Events;

namespace Zentin.Payment.Domain.Entities.Member
{
    public sealed class MbrInfo : AggregateRoot
    {
        public string MbrNo { get; private set; } = string.Empty;
        public string MbrName { get; private set; } = string.Empty;
        public string MchNo { get; private set; } = string.Empty;
        public string? MbrTel { get; private set; }
        public long Balance { get; private set; }
        public string? AliUserId { get; private set; }
        public string? WxMpOpenId { get; private set; }
        public string? AvatarUrl { get; private set; }
        public string? Remark { get; private set; }
        public MbrState State { get; private set; }
        public long? CreatedUid { get; private set; }
        public string? CreatedBy { get; private set; }
        


        public IReadOnlyCollection<MbrBill> Bills => _bills.AsReadOnly();
        private readonly List<MbrBill> _bills = [];

        private MbrInfo() { }

        
        public static MbrInfo Create(
            string mbrNo,
            string mbrName,
            string mchNo,
            string? mbrTel = null,
            long? createdUid = null,
            string? createdBy = null)
        {
            if (string.IsNullOrWhiteSpace(mbrNo))
                throw new ArgumentException("Member No is required", nameof(mbrNo));

            var mbr = new MbrInfo
            {
                MbrNo = mbrNo,
                MbrName = mbrName,
                MchNo = mchNo,
                MbrTel = mbrTel,
                Balance = 0,
                State = MbrState.Normal,
                CreatedUid = createdUid,
                CreatedBy = createdBy
            };

            mbr.AddDomainEvent(new MbrInfoCreatedDomainEvent(mbrNo, mchNo, mbrTel ?? string.Empty, DateTime.UtcNow));
            return mbr;
        }

        /// <summary>
        /// balance change(/recharge/consume/refund)
        /// </summary>
        /// <param name="changeAmount"></param>
        /// <param name="bizType"></param>
        /// <param name="remark"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void ChangeBalance(long changeAmount, MbrBillBizType bizType, string? remark = null)
        {
            if (State != MbrState.Normal)
                throw new InvalidOperationException("Member status is abnormal, cannot operate balance");

            var beforeBalance = Balance;
            var afterBalance = Balance + changeAmount;

            if (afterBalance < 0)
                throw new InvalidOperationException("Member balance is insufficient");

            Balance = afterBalance;

           
            // Create a bill record
            var bill = MbrBill.Create(
                Guid.NewGuid().ToString("N")[..30],
                MbrNo,
                MbrName,
                MchNo,
                MbrTel,
                beforeBalance,
                changeAmount,
                afterBalance,
                bizType,
                remark);

            _bills.Add(bill);
        }
    }
}
