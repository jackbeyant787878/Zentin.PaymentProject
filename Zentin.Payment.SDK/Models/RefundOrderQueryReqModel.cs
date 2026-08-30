using Newtonsoft.Json;

namespace Zentin.Payment.SDK.Models
{
    /// <summary>
    /// Refund order query request entity class
    /// </summary>
    public class RefundOrderQueryReqModel : AgPayObject
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
        /// Merchant refund order number
        /// </summary>
        [JsonProperty("mchRefundNo")]
        public string MchRefundNo { get; set; }

        /// <summary>
        /// Payment system refund order number
        /// </summary>
        [JsonProperty("refundOrderId")]
        public string RefundOrderId { get; set; }
    }
}