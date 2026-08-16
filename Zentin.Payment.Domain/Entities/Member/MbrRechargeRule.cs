
using Zentin.Payment.Common;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Domain.Entities.Member
{

    // <summary>
    /// Membership recharge rules entity (sub-entity within the merchant aggregation)
    /// </summary>
    public sealed class MbrRechargeRule : BaseEntity
    {
      
        public long RuleId { get; private set; }

        /// <summary>
        /// Merchant account number
        /// </summary>
        public string MchNo { get; private set; } = string.Empty;

        /// <summary>
        /// Recharge amount (unit: cents)
        /// </summary>
        public long RechargeAmount { get; private set; }

        /// <summary>
        /// Give amount (unit: cents)
        /// </summary>
        public long GiveAmount { get; private set; }

        /// <summary>
        /// Sort value (ascending)
        /// </summary>
        public long Sort { get; private set; }

        /// <summary>
        /// Remarks
        /// </summary>
        public string? Remark { get; private set; }

        /// <summary>
        /// State: 0-Disabled, 1-Normal
        /// </summary>
        public MbrState State { get; private set; }

        
        private MbrRechargeRule()
        {
            RuleId = SnowflakeIdGenerator.GetSnowflakeId();
        }

     
        /// <summary>
        /// create recharge rule
        /// </summary>
        /// <param name="mchNo">Merchant account number</param>
        /// <param name="rechargeAmount">Recharge amount (cents)</param>
        /// <param name="giveAmount">Give amount (cents)</param>
        /// <param name="sort">Sort value</param>
        /// <param name="remark">Remarks</param>
        /// <returns>Valid recharge rule entity</returns>
        public static MbrRechargeRule Create(
            string mchNo,
            long rechargeAmount,
            long giveAmount,
            long sort = 0,
            string? remark = null)
        {
           
            if (string.IsNullOrWhiteSpace(mchNo))
                throw new ArgumentException("Merchant account number cannot be empty", nameof(mchNo));
            if (rechargeAmount <= 0)
                throw new ArgumentException("Recharge amount must be greater than 0", nameof(rechargeAmount));
            if (giveAmount < 0)
                throw new ArgumentException(" Give amount cannot be negative", nameof(giveAmount));

            var rule = new MbrRechargeRule
            {
                MchNo = mchNo,
                RechargeAmount = rechargeAmount,
                GiveAmount = giveAmount,
                Sort = sort,
                Remark = remark,
                // default creation is enabled
                State = MbrState.Normal
            };

            return rule;
        }


        /// <summary>
        /// update rule content
        /// </summary>
        public void Update(
            long rechargeAmount,
            long giveAmount,
            long sort,
            string? remark)
        {
            if (rechargeAmount <= 0)
                throw new ArgumentException("Recharge amount must be greater than 0 ", nameof(rechargeAmount));
            if (giveAmount < 0)
                throw new ArgumentException("Give amount cannot be negative", nameof(giveAmount));

            RechargeAmount = rechargeAmount;
            GiveAmount = giveAmount;
            Sort = sort;
            Remark = remark;
        }

        /// <summary>
        /// enable rule
        /// </summary>
        public void Enable()
        {
            if (State == MbrState.Normal) return;
            State = MbrState.Normal;
        }

        /// <summary>
        /// disable rule
        /// </summary>
        public void Disable()
        {
            if (State == MbrState.Disabled) return;
            State = MbrState.Disabled;
        }
    }


}
