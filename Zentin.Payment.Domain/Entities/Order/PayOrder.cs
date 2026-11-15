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

        public IReadOnlyCollection<RefundOrder> RefundOrders => _refundOrders.AsReadOnly();
        private readonly List<RefundOrder> _refundOrders = [];

        public IReadOnlyCollection<PayOrderProfit> ProfitRecords => _profitRecords.AsReadOnly();
        private readonly List<PayOrderProfit> _profitRecords = [];

        public IReadOnlyCollection<PayOrderDivisionRecord> DivisionRecords => _divisionRecords.AsReadOnly();
        private readonly List<PayOrderDivisionRecord> _divisionRecords = [];

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
            string currency = "CNY",
            string? mchShortName = null,
            string? agentNo = null,
            string? agentName = null,
            string? isvNo = null,
            string? appName = null,
            long? storeId = null,
            int mchType = 1,
            string? clientIp = null,
            string? extParam = null,
            string? returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(payOrderId))
                throw new ArgumentException("Pay Order ID cannot be null or empty", nameof(payOrderId));
            if (string.IsNullOrWhiteSpace(mchNo))
                throw new ArgumentException("Merchant ID cannot be null or empty", nameof(mchNo));
            if (amount <= 0)
                throw new ArgumentException("Payment amount must be greater than 0", nameof(amount));
            if (string.IsNullOrWhiteSpace(notifyUrl))
                throw new ArgumentException("Notify URL cannot be null or empty", nameof(notifyUrl));

            return new PayOrder
            {
                PayOrderId = payOrderId,
                MchNo = mchNo,
                MchName = mchName,
                MchShortName = mchShortName,
                AgentNo = agentNo,
                AgentName = agentName,
                IsvNo = isvNo,
                AppId = appId,
                AppName = appName,
                StoreId = storeId,
                MchType = mchType,
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
                ReturnUrl = returnUrl,
                ExpiredTime = expiredTime,
                Currency = currency,
                ClientIp = clientIp,
                ExtParam = extParam,
                State = PayOrderStatus.Created,
                NotifyState = false,
                RefundState = RefundStatus.NoRefund,
                RefundTimes = 0,
                RefundAmount = 0,
                DivisionMode = 0,
                DivisionState = 0
            };
        }

        public void MarkPaying(string ifCode, string? channelExtra = null)
        {
            if (State != PayOrderStatus.Created)
                throw new InvalidOperationException("only created orders can be marked as paying");

            State = PayOrderStatus.Paying;
            IfCode = ifCode;
            ChannelExtra = channelExtra;
        }

        public void MarkSuccess(string channelOrderNo, long paidAmount, string? channelExtra = null)
        {
            if (State is PayOrderStatus.Success or PayOrderStatus.Closed or PayOrderStatus.Refunded)
                throw new InvalidOperationException("current order status cannot be marked as payment successful");

            State = PayOrderStatus.Success;
            ChannelOrderNo = channelOrderNo;
            SuccessTime = DateTime.UtcNow;
            ChannelExtra = channelExtra;

            AddDomainEvent(new PayOrderPaidDomainEvent(PayOrderId, MchNo, Amount, DateTime.UtcNow));
        }

        public void MarkFail(string errCode, string errMsg)
        {
            if (State is PayOrderStatus.Success or PayOrderStatus.Closed or PayOrderStatus.Refunded)
                throw new InvalidOperationException("current order status cannot be marked as payment failed");

            State = PayOrderStatus.Failed;
            ErrCode = errCode;
            ErrMsg = errMsg;
        }

        public void Close(string? reason = null)
        {
            if (State == PayOrderStatus.Closed)
                throw new InvalidOperationException("Order is already closed");
            if (State == PayOrderStatus.Success)
                throw new InvalidOperationException("Paid order cannot be closed");

            State = PayOrderStatus.Closed;
            ErrMsg = reason;
        }

        public void RecordRefund(long refundAmount)
        {
            if (State != PayOrderStatus.Success)
                throw new InvalidOperationException("only paid orders can initiate refunds");
            if (refundAmount <= 0)
                throw new ArgumentException("refund amount must be greater than 0", nameof(refundAmount));
            if (RefundAmount + refundAmount > Amount)
                throw new InvalidOperationException("refund amount exceeds the available refundable amount");

            RefundAmount += refundAmount;
            RefundTimes += 1;

            RefundState = RefundAmount == Amount
                ? RefundStatus.Full
                : RefundStatus.Partial;

            if (RefundState == RefundStatus.Full)
                State = PayOrderStatus.Refunded;
        }

        public void UpdateNotifyState(bool notifyState)
        {
            NotifyState = notifyState;
        }

        public PayOrderProfit AddProfitRecord(
            string infoId,
            string infoName,
            string infoType,
            decimal feeRate,
            long feeAmount,
            long orderFeeAmount,
            decimal profitRate,
            long profitAmount,
            long orderProfitAmount,
            string? feeRateDesc = null)
        {
            var record = PayOrderProfit.Create(infoId, infoName, infoType, PayOrderId, feeRate,
                feeAmount, orderFeeAmount, profitRate, profitAmount, orderProfitAmount, feeRateDesc);

            _profitRecords.Add(record);
            return record;
        }

        public PayOrderDivisionRecord AddDivisionRecord(
            string batchOrderId,
            long receiverId,
            DivisionReceiverAccType accType,
            string accNo,
            string accName,
            string relationType,
            string relationTypeName,
            decimal divisionProfit,
            long calDivisionAmount,
            long? receiverGroupId = null,
            string? receiverAlias = null)
        {
            if (State != PayOrderStatus.Success)
                throw new InvalidOperationException("only paid orders can be divided");

            var record = PayOrderDivisionRecord.Create(MchNo, AppId, IfCode ?? string.Empty, MchName, MchType,
                PayOrderId, Amount, calDivisionAmount, batchOrderId, receiverId, accType, accNo, accName,
                relationType, relationTypeName, divisionProfit, calDivisionAmount, IsvNo, ChannelOrderNo,
                receiverGroupId, receiverAlias);

            _divisionRecords.Add(record);
            return record;
        }
    }
}