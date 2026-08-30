using Newtonsoft.Json;

namespace Zentin.Payment.SDK.Models
{
    /// <summary>
    /// Payment query request entity class
    /// </summary>
    public class PayOrderQueryReqModel : AgPayObject
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
        /// Payment order number
        /// </summary>
        [JsonProperty("payOrderId")]
        public string PayOrderId { get; set; }
    }
}