using Newtonsoft.Json;

namespace Zentin.Payment.SDK.Models
{
    /// <summary>
    /// Payment order creation request entity class
    /// </summary>
    public class PayOrderCreateReqModel : AgPayObject
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
        /// Payment method
        /// </summary>
        [JsonProperty("wayCode")]
        public string WayCode { get; set; }

        /// <summary>
        /// Payment amount
        /// </summary>
        [JsonProperty("amount")]
        public long Amount { get; set; }

        /// <summary>
        /// Currency code, currently only supports cny
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Client IP
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
        /// Seller remark
        /// </summary>
        [JsonProperty("sellerRemark")]
        public string SellerRemark { get; set; }

        /// <summary>
        /// Buyer remark
        /// </summary>
        [JsonProperty("buyerRemark")]
        public string BuyerRemark { get; set; }

        /// <summary>
        /// Store ID
        /// </summary>
        [JsonProperty("storeId")]
        public long? StoreId { get; set; }

        /// <summary>
        /// QR code ID
        /// </summary>
        [JsonProperty("qrcId")]
        public string QrcId { get; set; }

        /// <summary>
        /// Asynchronous notification URL
        /// </summary>
        [JsonProperty("notifyUrl")]
        public string NotifyUrl { get; set; }

        /// <summary>
        /// Redirect notification URL
        /// </summary>
        [JsonProperty("returnUrl")]
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Order expiration time
        /// </summary>
        [JsonProperty("expiredTime")]
        public string ExpiredTime { get; set; }

        /// <summary>
        /// Specific channel additional payment parameters
        /// </summary>
        [JsonProperty("channelExtra")]
        public string ChannelExtra { get; set; }

        /// <summary>
        /// Channel user identifier, such as WeChat openId, Alipay account
        /// </summary>
        [JsonProperty("channelUser")]
        public string ChannelUser { get; set; }

        /// <summary>
        /// Merchant extension parameters
        /// </summary>
        [JsonProperty("extParam")]
        public string ExtParam { get; set; }

        /// <summary>
        /// Division mode: 0-This order does not allow division [default], 1-Automatically complete division according to configuration after payment success, 2-Manual division by merchant (unfreeze merchant amount)
        /// </summary>
        [JsonProperty("divisionMode")]
        public byte DivisionMode { get; set; }
    }
}