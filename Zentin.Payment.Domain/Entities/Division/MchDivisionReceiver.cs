using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Common;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Enums;
using static Zentin.Payment.Domain.Enums.Enums;

namespace Zentin.Payment.Domain.Entities.Division
{
    /// <summary>
    /// Merchant division receiver binding
    /// </summary>
    public sealed class MchDivisionReceiver : BaseEntity
    {
        public long ReceiverId { get; private set; }
        public string ReceiverAlias { get; private set; } = string.Empty;
        public long? ReceiverGroupId { get; private set; }
        public string? ReceiverGroupName { get; private set; }
        public string MchNo { get; private set; } = string.Empty;
        public string? IsvNo { get; private set; }
        public string AppId { get; private set; } = string.Empty;
        public string IfCode { get; private set; } = string.Empty;
        public DivisionReceiverAccType AccType { get; private set; }
        public string AccNo { get; private set; } = string.Empty;
        public string AccName { get; private set; } = string.Empty;
        public string RelationType { get; private set; } = string.Empty;
        public string RelationTypeName { get; private set; } = string.Empty;
        public decimal? DivisionProfit { get; private set; }
        public DivisionReceiverState State { get; private set; }
        public string? ChannelAccNo { get; private set; }
        public string? ChannelBindResult { get; private set; }
        public string? ChannelExtInfo { get; private set; }
        public DateTime? BindSuccessTime { get; private set; }

        private MchDivisionReceiver()
        {
            ReceiverId = SnowflakeIdGenerator.GetSnowflakeId();
        }

        /// <summary>
        /// Create a division receiver account
        /// </summary>
        public static MchDivisionReceiver Create(
            string receiverAlias,
            string mchNo,
            string appId,
            string ifCode,
            DivisionReceiverAccType accType,
            string accNo,
            string accName,
            string relationType,
            string relationTypeName,
            decimal? divisionProfit = null,
            long? receiverGroupId = null,
            string? receiverGroupName = null,
            string? isvNo = null)
        {
            if (string.IsNullOrWhiteSpace(receiverAlias))
                throw new ArgumentException("Receiver alias cannot be empty", nameof(receiverAlias));
            if (string.IsNullOrWhiteSpace(accNo))
                throw new ArgumentException("Account number cannot be empty", nameof(accNo));
            if (divisionProfit.HasValue && divisionProfit is < 0 or > 1)
                throw new ArgumentOutOfRangeException(nameof(divisionProfit), "Division ratio must be between 0 and 1");

            return new MchDivisionReceiver
            {
                ReceiverAlias = receiverAlias,
                MchNo = mchNo,
                AppId = appId,
                IfCode = ifCode,
                AccType = accType,
                AccNo = accNo,
                AccName = accName,
                RelationType = relationType,
                RelationTypeName = relationTypeName,
                DivisionProfit = divisionProfit,
                ReceiverGroupId = receiverGroupId,
                ReceiverGroupName = receiverGroupName,
                IsvNo = isvNo,
                State = DivisionReceiverState.Normal
            };
        }

        /// <summary>
        /// Mark channel binding success
        /// </summary>
        public void MarkBindSuccess(string channelAccNo, string bindResult, string? extInfo = null)
        {
            ChannelAccNo = channelAccNo;
            ChannelBindResult = bindResult;
            ChannelExtInfo = extInfo;
            BindSuccessTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Suspend division for this receiver
        /// </summary>
        public void Suspend()
        {
            if (State == DivisionReceiverState.Suspended) return;
            State = DivisionReceiverState.Suspended;
        }

        /// <summary>
        /// Resume division for this receiver
        /// </summary>
        public void Resume()
        {
            if (State == DivisionReceiverState.Normal) return;
            State = DivisionReceiverState.Normal;
        }

        /// <summary>
        /// Update division profit ratio
        /// </summary>
        public void UpdateProfitRatio(decimal newRatio)
        {
            if (newRatio is < 0 or > 1)
                throw new ArgumentOutOfRangeException(nameof(newRatio), "Ratio must be between 0 and 1");
            DivisionProfit = newRatio;
        }
    }
}
