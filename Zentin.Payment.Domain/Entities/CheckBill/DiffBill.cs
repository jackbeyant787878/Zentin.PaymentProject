using Zentin.Payment.Common;
using Zentin.Payment.Domain.Abstractions;
using static Zentin.Payment.Domain.Enums.Enums;

namespace Zentin.Payment.Domain.Entities.CheckBill
{
    /// <summary>
    /// Reconciliation difference record
    /// </summary>
    public sealed class DiffBill : BaseEntity
    {
        public long DiffBillId { get; private set; }
        public string DiffType { get; private set; } = string.Empty;
        public string IfCode { get; private set; } = string.Empty;
        public string BatchNo { get; private set; } = string.Empty;
        public string BillType { get; private set; } = string.Empty;
        public DateTime BillDate { get; private set; }
        public string OrderId { get; private set; } = string.Empty;
        public int OrderState { get; private set; }
        public DateTime? OrderSuccessTime { get; private set; }
        public DateTime OrderCreatedAt { get; private set; }
        public long Amount { get; private set; }
        public long FeeAmount { get; private set; }
        public long RefundAmount { get; private set; }
        public string? ChannelMchNo { get; private set; }
        public string? ChannelOrderNo { get; private set; }
        public long ChannelAmount { get; private set; }
        public long ChannelFeeAmount { get; private set; }
        public long ChannelRefundAmount { get; private set; }
        public DateTime? ChannelSuccessTime { get; private set; }
        public DiffBillHandleState HandleState { get; private set; }
        public long? HandleUid { get; private set; }
        public string? HandleBy { get; private set; }

        private DiffBill()
        {
            DiffBillId = SnowflakeIdGenerator.GetSnowflakeId();
        }

        public static DiffBill Create(
            string diffType,
            string ifCode,
            string batchNo,
            string billType,
            DateTime billDate,
            string orderId,
            int orderState,
            DateTime orderCreatedAt,
            long amount,
            long feeAmount,
            long refundAmount,
            long channelAmount,
            long channelFeeAmount,
            long channelRefundAmount,
            DateTime? orderSuccessTime = null,
            string? channelMchNo = null,
            string? channelOrderNo = null,
            DateTime? channelSuccessTime = null)
        {
            return new DiffBill
            {
                DiffType = diffType,
                IfCode = ifCode,
                BatchNo = batchNo,
                BillType = billType,
                BillDate = billDate,
                OrderId = orderId,
                OrderState = orderState,
                OrderCreatedAt = orderCreatedAt,
                Amount = amount,
                FeeAmount = feeAmount,
                RefundAmount = refundAmount,
                ChannelAmount = channelAmount,
                ChannelFeeAmount = channelFeeAmount,
                ChannelRefundAmount = channelRefundAmount,
                OrderSuccessTime = orderSuccessTime,
                ChannelMchNo = channelMchNo,
                ChannelOrderNo = channelOrderNo,
                ChannelSuccessTime = channelSuccessTime,
                HandleState = DiffBillHandleState.Unhandled
            };
        }

        public void MarkSuspended(long handleUid, string handleBy)
        {
            HandleState = DiffBillHandleState.Suspended;
            HandleUid = handleUid;
            HandleBy = handleBy;
        }

        public void MarkHandled(long handleUid, string handleBy)
        {
            HandleState = DiffBillHandleState.Handled;
            HandleUid = handleUid;
            HandleBy = handleBy;
        }

        public void MarkIgnored(long handleUid, string handleBy)
        {
            HandleState = DiffBillHandleState.Ignored;
            HandleUid = handleUid;
            HandleBy = handleBy;
        }
    }
}
