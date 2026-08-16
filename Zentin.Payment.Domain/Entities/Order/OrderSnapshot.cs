using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using static Zentin.Payment.Domain.Enums.Enums;

namespace Zentin.Payment.Domain.Entities.Order
{
    /// <summary>
    /// Encrypted interface data snapshot for order
    /// </summary>
    public sealed class OrderSnapshot : BaseEntity
    {
        /// <summary>
        /// Order ID
        /// </summary>
        public string OrderId { get; private set; } = string.Empty;

        /// <summary>
        /// Order type: Pay / Refund
        /// </summary>
        public OrderSnapshotType OrderType { get; private set; }

        /// <summary>
        /// Downstream request data (encrypted)
        /// </summary>
        public string? MchReqData { get; private set; }

        /// <summary>
        /// Downstream request time
        /// </summary>
        public DateTime? MchReqTime { get; private set; }

        /// <summary>
        /// Response data sent to downstream (encrypted)
        /// </summary>
        public string? MchRespData { get; private set; }

        /// <summary>
        /// Response sent time
        /// </summary>
        public DateTime? MchRespTime { get; private set; }

        /// <summary>
        /// Request data sent to upstream channel (encrypted)
        /// </summary>
        public string? ChannelReqData { get; private set; }

        /// <summary>
        /// Upstream request time
        /// </summary>
        public DateTime? ChannelReqTime { get; private set; }

        /// <summary>
        /// Upstream channel response data (encrypted)
        /// </summary>
        public string? ChannelRespData { get; private set; }

        /// <summary>
        /// Upstream response received time
        /// </summary>
        public DateTime? ChannelRespTime { get; private set; }

        private OrderSnapshot() { }

        /// <summary>
        /// Create an empty order snapshot
        /// </summary>
        public static OrderSnapshot Create(string orderId, OrderSnapshotType orderType)
        {
            if (string.IsNullOrWhiteSpace(orderId))
                throw new ArgumentException("Order ID cannot be empty", nameof(orderId));

            return new OrderSnapshot
            {
                OrderId = orderId,
                OrderType = orderType
            };
        }

        /// <summary>
        /// Set merchant request snapshot
        /// </summary>
        public void SetMerchantRequest(string reqData, DateTime reqTime)
        {
            MchReqData = reqData;
            MchReqTime = reqTime;
        }

        /// <summary>
        /// Set merchant response snapshot
        /// </summary>
        public void SetMerchantResponse(string respData, DateTime respTime)
        {
            MchRespData = respData;
            MchRespTime = respTime;
        }

        /// <summary>
        /// Set channel request snapshot
        /// </summary>
        public void SetChannelRequest(string reqData, DateTime reqTime)
        {
            ChannelReqData = reqData;
            ChannelReqTime = reqTime;
        }

        /// <summary>
        /// Set channel response snapshot
        /// </summary>
        public void SetChannelResponse(string respData, DateTime respTime)
        {
            ChannelRespData = respData;
            ChannelRespTime = respTime;
        }
    }
}
