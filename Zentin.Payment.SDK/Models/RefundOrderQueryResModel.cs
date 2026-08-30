using Newtonsoft.Json;

namespace Zentin.Payment.SDK.Models
{
    /// <summary>
    /// Refund order query response entity class
    /// </summary>
    public class RefundOrderQueryResModel : AgPayObject
    {
        /// <summary>
        /// Refund order number (payment system generated order number)
        /// </summary>
        [JsonProperty("refundOrderId")]
        public string RefundOrderId { get; set; }

        /// <summary>
        /// Payment order number
        /// </summary>
        [JsonProperty("payOrderId")]
        public string PayOrderId { get; set; }

        /// <summary>
        /// Merchant number
        /// </summary>
        [JsonProperty("mchNo")]
        public string MchNo { get; set; }

        /// <summary>
        /// Application ID
        /// </summary>
        [JsonProperty("appId")]
        public string AppId { get; set; }

        /// <summary>
        /// Merchant refund order number
        /// </summary>
        [JsonProperty("mchRefundNo")]
        public string MchRefundNo { get; set; }

        /// <summary>
        /// Payment amount, unit: cents
        /// </summary>
        [JsonProperty("payAmount")]
        public long PayAmount { get; set; }

        /// <summary>
        /// Refund amount, unit: cents
        /// </summary>
        [JsonProperty("refundAmount")]
        public long RefundAmount { get; set; }

        /// <summary>
        /// Three-digit currency code, Chinese Yuan: CNY
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Refund status
        /// 0-Order created
        /// 1-Refunding
        /// 2-Refund successful
        /// 3-Refund failed
        /// 4-Refund closed
        /// </summary>
        [JsonProperty("state")]
        public byte State { get; set; }

        /// <summary>
        /// Channel order number
        /// </summary>
        [JsonProperty("channelOrderNo")]
        public string ChannelOrderNo { get; set; }

        /// <summary>
        /// Channel error code
        /// </summary>
        [JsonProperty("errCode")]
        public string ErrCode { get; set; }

        /// <summary>
        /// Channel error description
        /// </summary>
        [JsonProperty("errMsg")]
        public string ErrMsg { get; set; }

        /// <summary>
        /// Extension parameter
        /// </summary>
        [JsonProperty("extParam")]
        public string ExtParam { get; set; }

        /// <summary>
        /// Order creation time, 13-digit timestamp
        /// </summary>
        [JsonProperty("createdAt")]
        public long CreatedAt { get; set; }

        /// <summary>
        /// Order payment success time, 13-digit timestamp
        /// </summary>
        [JsonProperty("successTime")]
        public long SuccessTime { get; set; }
    }
}