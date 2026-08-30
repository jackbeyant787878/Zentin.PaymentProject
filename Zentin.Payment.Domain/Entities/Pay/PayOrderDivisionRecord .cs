using Zentin.Payment.Common;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Domain.Entities.Pay
{
    /// <summary>
    /// Pay order division record (child entity of PayOrder)
    /// </summary>
    public sealed class PayOrderDivisionRecord : BaseEntity
    {
        public long PayOrderDivisionRecordId { get; private set; }
        public string MchNo { get; private set; } = string.Empty;
        public string? IsvNo { get; private set; }
        public string AppId { get; private set; } = string.Empty;
        public string MchName { get; private set; } = string.Empty;
        public int MchType { get; private set; }
        public string IfCode { get; private set; } = string.Empty;
        public string PayOrderId { get; private set; } = string.Empty;
        public string? PayOrderChannelOrderNo { get; private set; }
        public long PayOrderAmount { get; private set; }
        public long PayOrderDivisionAmount { get; private set; }
        public string BatchOrderId { get; private set; } = string.Empty;
        public string? ChannelBatchOrderId { get; private set; }
        public DivisionRecordState State { get; private set; }
        public string? ChannelRespResult { get; private set; }
        public long ReceiverId { get; private set; }
        public long? ReceiverGroupId { get; private set; }
        public string? ReceiverAlias { get; private set; }
        public DivisionReceiverAccType AccType { get; private set; }
        public string AccNo { get; private set; } = string.Empty;
        public string AccName { get; private set; } = string.Empty;
        public string RelationType { get; private set; } = string.Empty;
        public string RelationTypeName { get; private set; } = string.Empty;
        public decimal DivisionProfit { get; private set; }
        public long CalDivisionAmount { get; private set; }

        public PayOrder PayOrder { get; private set; } = null!;

        private PayOrderDivisionRecord()
        {
            PayOrderDivisionRecordId = SnowflakeIdGenerator.GetSnowflakeId();
        }

        internal static PayOrderDivisionRecord Create(
            string mchNo,
            string appId,
            string ifCode,
            string mchName,
            int mchType,
            string payOrderId,
            long payOrderAmount,
            long payOrderDivisionAmount,
            string batchOrderId,
            long receiverId,
            DivisionReceiverAccType accType,
            string accNo,
            string accName,
            string relationType,
            string relationTypeName,
            decimal divisionProfit,
            long calDivisionAmount,
            string? isvNo = null,
            string? payOrderChannelOrderNo = null,
            long? receiverGroupId = null,
            string? receiverAlias = null)
        {
            if (calDivisionAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(calDivisionAmount), "Division amount must be greater than 0");
            if (divisionProfit is < 0 or > 1)
                throw new ArgumentOutOfRangeException(nameof(divisionProfit), "Division ratio must be between 0 and 1");

            return new PayOrderDivisionRecord
            {
                MchNo = mchNo,
                AppId = appId,
                IfCode = ifCode,
                MchName = mchName,
                MchType = mchType,
                PayOrderId = payOrderId,
                PayOrderAmount = payOrderAmount,
                PayOrderDivisionAmount = payOrderDivisionAmount,
                BatchOrderId = batchOrderId,
                ReceiverId = receiverId,
                AccType = accType,
                AccNo = accNo,
                AccName = accName,
                RelationType = relationType,
                RelationTypeName = relationTypeName,
                DivisionProfit = divisionProfit,
                CalDivisionAmount = calDivisionAmount,
                IsvNo = isvNo,
                PayOrderChannelOrderNo = payOrderChannelOrderNo,
                ReceiverGroupId = receiverGroupId,
                ReceiverAlias = receiverAlias,
                State = DivisionRecordState.Pending
            };
        }

        public void MarkAccepted(string channelBatchOrderId, string? channelRespResult = null)
        {
            if (State != DivisionRecordState.Pending)
                throw new InvalidOperationException("Only pending records can be marked as accepted");

            State = DivisionRecordState.Accepted;
            ChannelBatchOrderId = channelBatchOrderId;
            ChannelRespResult = channelRespResult;
        }

        public void MarkSuccess(string? channelRespResult = null)
        {
            if (State == DivisionRecordState.Success)
                throw new InvalidOperationException("Record already succeeded");

            State = DivisionRecordState.Success;
            if (channelRespResult != null) ChannelRespResult = channelRespResult;
        }

        public void MarkFailed(string errMsg, string? channelRespResult = null)
        {
            if (State == DivisionRecordState.Success)
                throw new InvalidOperationException("Cannot mark successful record as failed");

            State = DivisionRecordState.Failed;
            if (channelRespResult != null) ChannelRespResult = channelRespResult;
        }
    }
}
