using Zentin.Payment.Common;
using Zentin.Payment.Domain.Abstractions;

namespace Zentin.Payment.Domain.Entities.Pay
{
    public sealed class PayOrderProfit : BaseEntity
    {
        public long PayOrderProfitId { get; private set; }
        public string InfoId { get; private set; } = string.Empty;
        public string InfoName { get; private set; } = string.Empty;
        public string InfoType { get; private set; } = string.Empty;
        public string PayOrderId { get; private set; } = string.Empty;
        public decimal FeeRate { get; private set; }
        public string? FeeRateDesc { get; private set; }
        public long FeeAmount { get; private set; }
        public long OrderFeeAmount { get; private set; }
        public decimal ProfitRate { get; private set; }
        public long ProfitAmount { get; private set; }
        public long OrderProfitAmount { get; private set; }

        public PayOrder PayOrder { get; private set; } = null!;

        public PayOrderProfit()
        {
            PayOrderProfitId = SnowflakeIdGenerator.GetSnowflakeId();
        }


        #region internal factory methods (only callable by aggregate root)
        /// <summary>
        /// create profit record (called within the aggregate)
        /// </summary>
        internal static PayOrderProfit Create(string infoId, string infoName, string infoType, string payOrderId, decimal feeRate,
            long feeAmount, long orderFeeAmount, decimal profitRate, long profitAmount, long orderProfitAmount, string? feeRateDesc = null)
        {
            if (string.IsNullOrWhiteSpace(infoId))
                throw new ArgumentException("Profit Account ID is required", nameof(infoId));
            if (profitAmount < 0)
                throw new ArgumentException("Profit amount cannot be negative", nameof(profitAmount));

            return new PayOrderProfit
            {
                InfoId = infoId,
                InfoName = infoName,
                InfoType = infoType,
                PayOrderId = payOrderId,
                FeeRate = feeRate,
                FeeRateDesc = feeRateDesc,
                FeeAmount = feeAmount,
                OrderFeeAmount = orderFeeAmount,
                ProfitRate = profitRate,
                ProfitAmount = profitAmount,
                OrderProfitAmount = orderProfitAmount
            };
        }
        #endregion

    }
}
