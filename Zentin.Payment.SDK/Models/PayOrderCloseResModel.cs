using Newtonsoft.Json;

namespace Zentin.Payment.SDK.Models
{
    /// <summary>
    /// Close order response result
    /// </summary>
    public class PayOrderCloseResModel : AgPayObject
    {
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