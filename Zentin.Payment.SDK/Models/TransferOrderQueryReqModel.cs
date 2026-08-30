using Newtonsoft.Json;

namespace Zentin.Payment.SDK.Models
{
    /// <summary>
    /// Transfer order query request entity class
    /// </summary>
    public class TransferOrderQueryReqModel : AgPayObject
    {
        /// <summary>
        /// Transfer order number
        /// </summary>
        [JsonProperty("transferId")]
        public string TransferId { get; set; }

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
    }
}