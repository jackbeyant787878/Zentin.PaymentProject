using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Events;
using static Zentin.Payment.Domain.Enums.Enums;

namespace Zentin.Payment.Infrastructure.EntityTypeConfiguration.Pay
{
    /// <summary>
    /// Merchant transfer order aggregate root
    /// </summary>
    public sealed class TransferOrder : AggregateRoot
    {
        public string TransferId { get; private set; } = string.Empty;
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
        public int MchType { get; private set; }
        public string MchOrderNo { get; private set; } = string.Empty;
        public string IfCode { get; private set; } = string.Empty;
        public string EntryType { get; private set; } = string.Empty;
        public long Amount { get; private set; }
        public string Currency { get; private set; } = "CNY";
        public string AccountNo { get; private set; } = string.Empty;
        public string? AccountName { get; private set; }
        public string? BankName { get; private set; }
        public string TransferDesc { get; private set; } = string.Empty;
        public string? ClientIp { get; private set; }
        public TransferOrderStatus State { get; private set; }
        public string? ChannelExtra { get; private set; }
        public string? ChannelOrderNo { get; private set; }
        public string? ErrCode { get; private set; }
        public string? ErrMsg { get; private set; }
        public string? ExtParam { get; private set; }
        public string NotifyUrl { get; private set; } = string.Empty;
        public DateTime? SuccessTime { get; private set; }

        private TransferOrder() { }

        /// <summary>
        /// Create a new transfer order
        /// </summary>
        public static TransferOrder Create(
            string transferId,
            string mchNo,
            string mchName,
            string appId,
            string mchOrderNo,
            string ifCode,
            string entryType,
            long amount,
            string accountNo,
            string notifyUrl,
            string transferDesc = "",
            string currency = "CNY",
            int mchType = 1,
            string? accountName = null,
            string? bankName = null,
            string? clientIp = null,
            string? channelExtra = null,
            string? extParam = null)
        {
            if (string.IsNullOrWhiteSpace(transferId))
                throw new ArgumentException("Transfer ID cannot be empty", nameof(transferId));
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Transfer amount must be greater than 0");
            if (string.IsNullOrWhiteSpace(accountNo))
                throw new ArgumentException("Receiver account cannot be empty", nameof(accountNo));

            return new TransferOrder
            {
                TransferId = transferId,
                MchNo = mchNo,
                MchName = mchName,
                AppId = appId,
                MchOrderNo = mchOrderNo,
                IfCode = ifCode,
                EntryType = entryType,
                Amount = amount,
                AccountNo = accountNo,
                NotifyUrl = notifyUrl,
                TransferDesc = transferDesc,
                Currency = currency,
                MchType = mchType,
                AccountName = accountName,
                BankName = bankName,
                ClientIp = clientIp,
                ChannelExtra = channelExtra,
                ExtParam = extParam,
                State = TransferOrderStatus.Created
            };
        }

        /// <summary>
        /// Mark transfer submitted to channel
        /// </summary>
        public void MarkProcessing(string channelOrderNo)
        {
            if (State != TransferOrderStatus.Created)
                throw new InvalidOperationException("Only created transfer can be submitted");

            State = TransferOrderStatus.Processing;
            ChannelOrderNo = channelOrderNo;
        }

        /// <summary>
        /// Mark transfer success
        /// </summary>
        public void MarkSuccess()
        {
            if (State is TransferOrderStatus.Success or TransferOrderStatus.Closed)
                throw new InvalidOperationException("Invalid state transition");

            State = TransferOrderStatus.Success;
            SuccessTime = DateTime.UtcNow;

            AddDomainEvent(new TransferSuccessDomainEvent(TransferId, MchNo, Amount, SuccessTime.Value));
        }

        /// <summary>
        /// Mark transfer failed
        /// </summary>
        public void MarkFailed(string errCode, string errMsg)
        {
            if (State is TransferOrderStatus.Success or TransferOrderStatus.Closed)
                throw new InvalidOperationException("Cannot mark successful transfer as failed");

            State = TransferOrderStatus.Failed;
            ErrCode = errCode;
            ErrMsg = errMsg;
        }

        /// <summary>
        /// Close transfer order
        /// </summary>
        public void Close()
        {
            if (State == TransferOrderStatus.Closed)
                throw new InvalidOperationException("Transfer already closed");
            State = TransferOrderStatus.Closed;
        }
    }
}
