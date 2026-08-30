using Newtonsoft.Json;

namespace Zentin.Payment.SDK.Models
{
    /// <summary>
    /// Transfer order creation request entity class
    /// </summary>
    public class TransferOrderCreateReqModel : AgPayObject
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
        /// Payment interface code
        /// </summary>
        [JsonProperty("ifCode")]
        public string IfCode { get; set; }

        /// <summary>
        /// Entry method
        /// </summary>
        [JsonProperty("entryType")]
        public string EntryType { get; set; }

        /// <summary>
        /// Transfer amount
        /// </summary>
        [JsonProperty("amount")]
        public long Amount { get; set; }

        /// <summary>
        /// Currency code, currently only supports cny
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Receiving account number
        /// </summary>
        [JsonProperty("accountNo")]
        public string AccountNo { get; set; }

        /// <summary>
        /// Recipient name
        /// </summary>
        [JsonProperty("accountName")]
        public string AccountName { get; set; }

        /// <summary>
        /// Recipient bank name
        /// </summary>
        [JsonProperty("bankName")]
        public string BankName { get; set; }

        /// <summary>
        /// Client IP
        /// </summary>
        [JsonProperty("clientIp")]
        public string ClientIp { get; set; }

        /// <summary>
        /// Transfer description
        /// </summary>
        [JsonProperty("transferDesc")]
        public string TransferDesc { get; set; }

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