using Newtonsoft.Json;

namespace Zentin.Payment.SDK.Models
{
    /// <summary>
    /// Payment query response entity class
    /// </summary>
    public class PayOrderQueryResModel : AgPayObject
    {
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
        /// Merchant order number
        /// </summary>
        [JsonProperty("mchOrderNo")]
        public string MchOrderNo { get; set; }

        /// <summary>
        /// Payment interface
        /// </summary>
        [JsonProperty("ifCode")]
        public string IfCode { get; set; }

        /// <summary>
        /// Payment method
        /// </summary>
        [JsonProperty("wayCode")]
        public string WayCode { get; set; }

        /// <summary>
        /// Payment amount, unit: cent
        /// </summary>
        [JsonProperty("amount")]
        public long Amount { get; set; }

        /// <summary>
        /// Three-digit currency code, Chinese Yuan: CNY
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Payment order status
        /// 0-Order created
        /// 1-Paying
        /// 2-Payment successful
        /// 3-Payment failed
        /// 4-Cancelled
        /// 5-Refunded
        /// 6-Order closed
        /// </summary>
        [JsonProperty("state")]
        public int State { get; set; }

        /// <summary>
        /// Client IPv4 address
        /// </summary>
        [JsonProperty("clientIp")]
        public string ClientIp { get; set; }

        /// <summary>
        /// Product title
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Product description
        /// </summary>
        [JsonProperty("body")]
        public string Body { get; set; }

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