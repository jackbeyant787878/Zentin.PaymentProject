using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using static Zentin.Payment.Domain.Enums.Enums;

namespace Zentin.Payment.Domain.Entities.CheckBill
{
    /// <summary>
    /// Reconciliation batch record
    /// </summary>
    public sealed class CheckBillBatch : BaseEntity
    {
        public string BatchNo { get; private set; } = string.Empty;
        public string IfCode { get; private set; } = string.Empty;
        public DateTime BillDate { get; private set; }
        public string? OrgBillFilePath { get; private set; }
        public CheckBillParseStatus ParseStatus { get; private set; }
        public string? ParseErrMsg { get; private set; }
        public CheckBillHandleState HandleState { get; private set; }
        public int DiffCount { get; private set; }
        public int UnHandleCount { get; private set; }
        public long TotalAmount { get; private set; }
        public int TotalCount { get; private set; }
        public long TotalFeeAmount { get; private set; }
        public long TotalRefundAmount { get; private set; }
        public int TotalRefundCount { get; private set; }
        public string? ChannelMchNo { get; private set; }
        public long ChannelTotalAmount { get; private set; }
        public int ChannelTotalCount { get; private set; }
        public long ChannelTotalFeeAmount { get; private set; }
        public long ChannelTotalRefundAmount { get; private set; }
        public int ChannelTotalRefundCount { get; private set; }

        private CheckBillBatch() { }

        public static CheckBillBatch Create(
            string batchNo,
            string ifCode,
            DateTime billDate,
            string? channelMchNo = null)
        {
            if (string.IsNullOrWhiteSpace(batchNo))
                throw new ArgumentException("Batch number cannot be empty", nameof(batchNo));

            return new CheckBillBatch
            {
                BatchNo = batchNo,
                IfCode = ifCode,
                BillDate = billDate,
                ChannelMchNo = channelMchNo,
                ParseStatus = CheckBillParseStatus.Failed,
                HandleState = CheckBillHandleState.Unhandled
            };
        }

        public void MarkParseSuccess(string filePath)
        {
            ParseStatus = CheckBillParseStatus.Success;
            OrgBillFilePath = filePath;
            ParseErrMsg = null;
        }

        public void MarkParseFailed(string errMsg)
        {
            ParseStatus = CheckBillParseStatus.Failed;
            ParseErrMsg = errMsg;
        }

        public void SetStatistics(
            long totalAmount,
            int totalCount,
            long totalFeeAmount,
            long totalRefundAmount,
            int totalRefundCount,
            long channelTotalAmount,
            int channelTotalCount,
            long channelTotalFeeAmount,
            long channelTotalRefundAmount,
            int channelTotalRefundCount,
            int diffCount,
            int unHandleCount)
        {
            TotalAmount = totalAmount;
            TotalCount = totalCount;
            TotalFeeAmount = totalFeeAmount;
            TotalRefundAmount = totalRefundAmount;
            TotalRefundCount = totalRefundCount;
            ChannelTotalAmount = channelTotalAmount;
            ChannelTotalCount = channelTotalCount;
            ChannelTotalFeeAmount = channelTotalFeeAmount;
            ChannelTotalRefundAmount = channelTotalRefundAmount;
            ChannelTotalRefundCount = channelTotalRefundCount;
            DiffCount = diffCount;
            UnHandleCount = unHandleCount;
        }

        public void MarkHandled()
        {
            HandleState = CheckBillHandleState.Handled;
            UnHandleCount = 0;
        }
    }
}
