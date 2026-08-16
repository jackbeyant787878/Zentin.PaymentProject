using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Common;
using Zentin.Payment.Domain.Abstractions;

namespace Zentin.Payment.Domain.Entities.CheckBill
{
    /// <summary>
    /// Original channel bill record
    /// </summary>
    public sealed class ChannelBill : BaseEntity
    {
        public long ChannelBillId { get; private set; }
        public string IfCode { get; private set; } = string.Empty;
        public string BatchNo { get; private set; } = string.Empty;
        public string BillType { get; private set; } = string.Empty;
        public DateTime BillDate { get; private set; }
        public string OrderId { get; private set; } = string.Empty;
        public string? ChannelMchNo { get; private set; }
        public string? ChannelOrderNo { get; private set; }
        public bool ChannelState { get; private set; }
        public long ChannelAmount { get; private set; }
        public long ChannelFeeAmount { get; private set; }
        public DateTime? ChannelSuccessTime { get; private set; }
        public string? ChannelUser { get; private set; }
        public string Currency { get; private set; } = "CNY";

        private ChannelBill()
        {
            ChannelBillId = SnowflakeIdGenerator.GetSnowflakeId();
        }

        public static ChannelBill Create(
            string ifCode,
            string batchNo,
            string billType,
            DateTime billDate,
            string orderId,
            bool channelState,
            long channelAmount,
            long channelFeeAmount,
            string currency = "CNY",
            string? channelMchNo = null,
            string? channelOrderNo = null,
            DateTime? channelSuccessTime = null,
            string? channelUser = null)
        {
            return new ChannelBill
            {
                IfCode = ifCode,
                BatchNo = batchNo,
                BillType = billType,
                BillDate = billDate,
                OrderId = orderId,
                ChannelState = channelState,
                ChannelAmount = channelAmount,
                ChannelFeeAmount = channelFeeAmount,
                Currency = currency,
                ChannelMchNo = channelMchNo,
                ChannelOrderNo = channelOrderNo,
                ChannelSuccessTime = channelSuccessTime,
                ChannelUser = channelUser
            };
        }
    }
}
