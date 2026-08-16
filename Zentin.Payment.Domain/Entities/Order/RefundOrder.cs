using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Enums;
using static Zentin.Payment.Domain.Enums.Enums;

namespace Zentin.Payment.Domain.Entities.Order
{
    /// <summary>
    /// Refund order aggregate root
    /// </summary>
    public sealed class RefundOrder : BaseEntity
    {
        public string RefundOrderId { get; private set; } = string.Empty;
        public string PayOrderId { get; private set; } = string.Empty;
        public string? ChannelPayOrderNo { get; private set; }
        public string MchNo { get; private set; } = string.Empty;
        public string MchName { get; private set; } = string.Empty;
        public string? MchShortName { get; private set; }
        public string? AgentNo { get; private set; }
        public string? AgentName { get; private set; }
        public string? AgentShortName { get; private set; }
        public string? IsvNo { get; private set; }
        public string? IsvName { get; private set; }
        public string? IsvShortName { get; private set; }
        public string AppId { get; private set; } = string.Empty;
        public string? AppName { get; private set; }
        public long? StoreId { get; private set; }
        public string? StoreName { get; private set; }
        public int MchType { get; private set; }
        public string MchRefundNo { get; private set; } = string.Empty;
        public string WayCode { get; private set; } = string.Empty;
        public string WayType { get; private set; } = string.Empty;
        public string IfCode { get; private set; } = string.Empty;
        public long PayAmount { get; private set; }
        public long RefundAmount { get; private set; }
        public long RefundFeeAmount { get; private set; }
        public string Currency { get; private set; } = "CNY";
        public RefundOrderStatus State { get; private set; }
        public string? ClientIp { get; private set; }
        public string RefundReason { get; private set; } = string.Empty;
        public string? ChannelOrderNo { get; private set; }
        public string? ErrCode { get; private set; }
        public string? ErrMsg { get; private set; }
        public string? ChannelExtra { get; private set; }
        public string? NotifyUrl { get; private set; }
        public string? ExtParam { get; private set; }
        public DateTime? SuccessTime { get; private set; }
        public DateTime? ExpiredTime { get; private set; }

        public PayOrder PayOrder { get; private set; } = null!;

       
        private RefundOrder() { }

        /// <summary>
        /// Create a new refund order with business validation
        /// </summary>
        public static RefundOrder Create(
            string refundOrderId,
            string payOrderId,
            string mchNo,
            string mchName,
            string appId,
            string mchRefundNo,
            string wayCode,
            string wayType,
            string ifCode,
            long payAmount,
            long refundAmount,
            long refundFeeAmount,
            string refundReason,
            string? notifyUrl = null,
            string currency = "CNY",
            int mchType = 1,
            string? clientIp = null,
            string? channelExtra = null,
            DateTime? expiredTime = null)
        {
            if (string.IsNullOrWhiteSpace(refundOrderId))
                throw new ArgumentException("Refund order ID cannot be empty", nameof(refundOrderId));
            if (refundAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(refundAmount), "Refund amount must be greater than 0");
            if (refundAmount > payAmount)
                throw new InvalidOperationException("Refund amount cannot exceed original payment amount");

            return new RefundOrder
            {
                RefundOrderId = refundOrderId,
                PayOrderId = payOrderId,
                MchNo = mchNo,
                MchName = mchName,
                AppId = appId,
                MchRefundNo = mchRefundNo,
                WayCode = wayCode,
                WayType = wayType,
                IfCode = ifCode,
                PayAmount = payAmount,
                RefundAmount = refundAmount,
                RefundFeeAmount = refundFeeAmount,
                RefundReason = refundReason,
                NotifyUrl = notifyUrl,
                Currency = currency,
                MchType = mchType,
                ClientIp = clientIp,
                ChannelExtra = channelExtra,
                ExpiredTime = expiredTime,
                State = RefundOrderStatus.Created
            };
        }

        /// <summary>
        /// Mark refund as submitted to channel (processing state)
        /// </summary>
        public void MarkProcessing(string channelOrderNo)
        {
            if (State != RefundOrderStatus.Created)
                throw new InvalidOperationException("Only created refund can be submitted to channel");

            State = RefundOrderStatus.Processing;
            ChannelOrderNo = channelOrderNo;
        }

        /// <summary>
        /// Mark refund confirmed successful
        /// </summary>
        public void MarkSuccess()
        {
            if (State is RefundOrderStatus.Success or RefundOrderStatus.Closed)
                throw new InvalidOperationException("Invalid state transition to success");

            State = RefundOrderStatus.Success;
            SuccessTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Mark refund failed with error info
        /// </summary>
        public void MarkFailed(string errCode, string errMsg)
        {
            if (State is RefundOrderStatus.Success or RefundOrderStatus.Closed)
                throw new InvalidOperationException("Cannot mark successful refund as failed");

            State = RefundOrderStatus.Failed;
            ErrCode = errCode;
            ErrMsg = errMsg;
        }

        /// <summary>
        /// Close expired or cancelled refund task
        /// </summary>
        public void Close()
        {
            if (State == RefundOrderStatus.Closed)
                throw new InvalidOperationException("Refund already closed");

            State = RefundOrderStatus.Closed;
        }
    }
}
