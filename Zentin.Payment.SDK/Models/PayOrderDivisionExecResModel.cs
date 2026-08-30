using Newtonsoft.Json;

namespace Zentin.Payment.SDK.Models
{
    /// <summary>
    /// Profit sharing response result
    /// </summary>
    public class PayOrderDivisionExecResModel : AgPayObject
    {
        /// <summary>
        /// Sharing status 1-Sharing successful, 2-Sharing failed
        /// </summary>
        [JsonProperty("state")]
        public byte State { get; set; }

        /// <summary>
        /// Upstream sharing batch order number
        /// </summary>
        [JsonProperty("channelBatchOrderId")]
        public string ChannelBatchOrderId { get; set; }

        /// <summary>
        /// Payment channel error code
        /// </summary>
        [JsonProperty("errCode")]
        public string ErrCode { get; set; }

        /// <summary>
        /// Payment channel error message
        /// </summary>
        [JsonProperty("errMsg")]
        public string ErrMsg { get; set; }
    }
}