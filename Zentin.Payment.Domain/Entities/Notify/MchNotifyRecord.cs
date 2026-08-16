using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Common;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Domain.Entities.Notify
{
    /// <summary>
    /// Merchant asynchronous notify record
    /// </summary>
    public sealed class MchNotifyRecord : BaseEntity
    {
        /// <summary>
        /// Notify record ID (snowflake)
        /// </summary>
        public long NotifyId { get; private set; }

        /// <summary>
        /// Associated order ID
        /// </summary>
        public string OrderId { get; private set; } = string.Empty;

        /// <summary>
        /// Order type: Pay / Refund
        /// </summary>
        public NotifyOrderType OrderType { get; private set; }

        /// <summary>
        /// Merchant order number
        /// </summary>
        public string MchOrderNo { get; private set; } = string.Empty;

        /// <summary>
        /// Merchant number
        /// </summary>
        public string MchNo { get; private set; } = string.Empty;

        /// <summary>
        /// Agent number
        /// </summary>
        public string? AgentNo { get; private set; }

        /// <summary>
        /// ISV number
        /// </summary>
        public string? IsvNo { get; private set; }

        /// <summary>
        /// Application ID
        /// </summary>
        public string AppId { get; private set; } = string.Empty;

        /// <summary>
        /// Notify callback URL
        /// </summary>
        public string NotifyUrl { get; private set; } = string.Empty;

        /// <summary>
        /// HTTP request method
        /// </summary>
        public string ReqMethod { get; private set; } = string.Empty;

        /// <summary>
        /// Request media type
        /// </summary>
        public string ReqMediaType { get; private set; } = string.Empty;

        /// <summary>
        /// Request body content
        /// </summary>
        public string? ReqBody { get; private set; }

        /// <summary>
        /// Response result from merchant
        /// </summary>
        public string? ResResult { get; private set; }

        /// <summary>
        /// Current notify retry count
        /// </summary>
        public int NotifyCount { get; private set; }

        /// <summary>
        /// Max notify retry limit
        /// </summary>
        public int NotifyCountLimit { get; private set; }

        /// <summary>
        /// Notify execution state
        /// </summary>
        public NotifyState State { get; private set; }

        /// <summary>
        /// Last notify sent time
        /// </summary>
        public DateTime? LastNotifyTime { get; private set; }

        private MchNotifyRecord()
        {
            NotifyId = SnowflakeIdGenerator.GetSnowflakeId();
        }

        /// <summary>
        /// Create a new merchant notify record
        /// </summary>
        public static MchNotifyRecord Create(
            string orderId,
            NotifyOrderType orderType,
            string mchOrderNo,
            string mchNo,
            string appId,
            string notifyUrl,
            string reqMethod,
            string reqMediaType,
            string? reqBody = null,
            string? agentNo = null,
            string? isvNo = null,
            int notifyCountLimit = 6)
        {
            if (string.IsNullOrWhiteSpace(orderId))
                throw new ArgumentException("Order ID cannot be empty", nameof(orderId));
            if (string.IsNullOrWhiteSpace(mchNo))
                throw new ArgumentException("Merchant number cannot be empty", nameof(mchNo));
            if (string.IsNullOrWhiteSpace(notifyUrl))
                throw new ArgumentException("Notify URL cannot be empty", nameof(notifyUrl));
            if (notifyCountLimit <= 0)
                throw new ArgumentOutOfRangeException(nameof(notifyCountLimit), "Retry limit must be greater than 0");

            return new MchNotifyRecord
            {
                OrderId = orderId,
                OrderType = orderType,
                MchOrderNo = mchOrderNo,
                MchNo = mchNo,
                AppId = appId,
                NotifyUrl = notifyUrl,
                ReqMethod = reqMethod,
                ReqMediaType = reqMediaType,
                ReqBody = reqBody,
                AgentNo = agentNo,
                IsvNo = isvNo,
                NotifyCountLimit = notifyCountLimit,
                NotifyCount = 0,
                State = NotifyState.Notifying
            };
        }

        /// <summary>
        /// Record one notify attempt and increment counter
        /// </summary>
        public void RecordAttempt()
        {
            if (State != NotifyState.Notifying)
                throw new InvalidOperationException("Only notifying records can retry");

            NotifyCount++;
            LastNotifyTime = DateTime.UtcNow;

            if (NotifyCount >= NotifyCountLimit)
                State = NotifyState.Failed;
        }

        /// <summary>
        /// Mark notify success
        /// </summary>
        public void MarkSuccess(string resResult)
        {
            if (State == NotifyState.Success)
                throw new InvalidOperationException("Notify already succeeded");

            State = NotifyState.Success;
            ResResult = resResult;
            LastNotifyTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Mark notify failed
        /// </summary>
        public void MarkFailed(string resResult)
        {
            State = NotifyState.Failed;
            ResResult = resResult;
            LastNotifyTime = DateTime.UtcNow;
        }
    }
}
