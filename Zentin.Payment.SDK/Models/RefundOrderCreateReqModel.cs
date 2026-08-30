using Newtonsoft.Json;

namespace Zentin.Payment.SDK.Models
{
    /// <summary>
    /// Refund order creation request entity class
    /// </summary>
    public class RefundOrderCreateReqModel : AgPayObject
    {
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
        /// Merchant order number
        /// </summary>
        [JsonProperty("mchOrderNo")]
        public string MchOrderNo { get; set; }

        /// <summary>
        /// Payment system order number
        /// </summary>
        [JsonProperty("payOrderId")]
        public string PayOrderId { get; set; }

        /// <summary>
        /// Refund order number
        /// </summary>
        [JsonProperty("mchRefundNo")]
        public string MchRefundNo { get; set; }

        /// <summary>
        /// Refund amount
        /// </summary>
        [JsonProperty("refundAmount")]
        public long RefundAmount { get; set; }

        /// <summary>
        /// Currency code, currently only supports cny
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Refund reason
        /// </summary>
        [JsonProperty("refundReason")]
        public string RefundReason { get; set; }

        /// <summary>
        /// Client IP
        /// </summary>
        [JsonProperty("clientIp")]
        public string ClientIp { get; set; }

        /// <summary>
        /// Asynchronous notification URL
        /// </summary>
        [JsonProperty("notifyUrl")]
        public string NotifyUrl { get; set; }

        /// <summary>
        /// Specific channel additional payment parameters
        /// </summary>
        [JsonProperty("channelExtra")]
        public string ChannelExtra { get; set; }

        /// <summary>
        /// Merchant extension parameters
        /// </summary>
        [JsonProperty("extParam")]
        public string ExtParam { get; set; }
    }
}