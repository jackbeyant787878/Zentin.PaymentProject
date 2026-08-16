using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Entities.Pay;
using Zentin.Payment.Domain.Enums;
using Zentin.Payment.Domain.Events;

namespace Zentin.Payment.Domain.Entities.Order
{
    public sealed class PayOrder : AggregateRoot
    {
        public string PayOrderId { get; private set; } = string.Empty;
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
        public string? QrcId { get; private set; }
        public int MchType { get; private set; }
        public string MchOrderNo { get; private set; } = string.Empty;
        public string? IfCode { get; private set; }
        public string WayCode { get; private set; } = string.Empty;
        public string WayType { get; private set; } = string.Empty;
        public long Amount { get; private set; }
        public decimal MchFeeRate { get; private set; }
        public string? MchFeeRateDesc { get; private set; }
        public long MchFeeAmount { get; private set; }
        public long MchOrderFeeAmount { get; private set; }
        public string Currency { get; private set; } = "CNY";
        public PayOrderStatus State { get; private set; }
        public bool NotifyState { get; private set; }
        public string? ClientIp { get; private set; }
        public string Subject { get; private set; } = string.Empty;
        public string Body { get; private set; } = string.Empty;
        public string? SellerRemark { get; private set; }
        public string? BuyerRemark { get; private set; }
        public string? ChannelMchNo { get; private set; }
        public string? ChannelIsvNo { get; private set; }
        public string? ChannelExtra { get; private set; }
        public string? ChannelUser { get; private set; }
        public string? ChannelOrderNo { get; private set; }
        public string? PlatformOrderNo { get; private set; }
        public string? PlatformMchOrderNo { get; private set; }
        public RefundStatus RefundState { get; private set; }
        public int RefundTimes { get; private set; }
        public long RefundAmount { get; private set; }
        public int DivisionMode { get; private set; }
        public int DivisionState { get; private set; }
        public DateTime? DivisionLastTime { get; private set; }
        public string? ErrCode { get; private set; }
        public string? ErrMsg { get; private set; }
        public string? ExtParam { get; private set; }
        public string NotifyUrl { get; private set; } = string.Empty;
        public string? ReturnUrl { get; private set; }
        public DateTime? ExpiredTime { get; private set; }
        public DateTime? SuccessTime { get; private set; }

        public ICollection<RefundOrder> RefundOrders { get; private set; } = new List<RefundOrder>();
        public ICollection<PayOrderProfit> ProfitRecords { get; private set; } = new List<PayOrderProfit>();
        public ICollection<PayOrderDivisionRecord> DivisionRecords { get; private set; } = new List<PayOrderDivisionRecord>();

        private PayOrder() { }

        public static PayOrder Create(
            string payOrderId,
            string mchNo,
            string mchName,
            string appId,
            string mchOrderNo,
            string wayCode,
            string wayType,
            long amount,
            decimal mchFeeRate,
            long mchFeeAmount,
            string subject,
            string body,
            string notifyUrl,
            DateTime expiredTime,
            string currency = "CNY")
        {
            return new PayOrder
            {
                PayOrderId = payOrderId,
                MchNo = mchNo,
                MchName = mchName,
                AppId = appId,
                MchOrderNo = mchOrderNo,
                WayCode = wayCode,
                WayType = wayType,
                Amount = amount,
                MchFeeRate = mchFeeRate,
                MchFeeAmount = mchFeeAmount,
                MchOrderFeeAmount = mchFeeAmount,
                Subject = subject,
                Body = body,
                NotifyUrl = notifyUrl,
                ExpiredTime = expiredTime,
                Currency = currency,
                State = PayOrderStatus.Created,
                RefundState = RefundStatus.NoRefund,
                NotifyState = false,
                RefundTimes = 0,
                RefundAmount = 0,
                DivisionMode = 0,
                DivisionState = 0
            };
        }

        public void MarkSuccess(string channelOrderNo, long paidAmount, string? channelExtra = null)
        {
            if (State is PayOrderStatus.Success or PayOrderStatus.Closed)
                throw new InvalidOperationException("Current order status does not allow marking as paid");

            State = PayOrderStatus.Success;
            ChannelOrderNo = channelOrderNo;
            SuccessTime = DateTime.UtcNow;
            ChannelExtra = channelExtra;

            AddDomainEvent(new PayOrderPaidDomainEvent(PayOrderId, MchNo, Amount,DateTime.UtcNow));
        }

        public void Close(string? reason = null)
        {
            if (State == PayOrderStatus.Closed)
                throw new InvalidOperationException("Order was already closed");

            State = PayOrderStatus.Closed;
            ErrMsg = reason;
        }
    }
}
