using Newtonsoft.Json;

namespace Zentin.Payment.SDK.Models
{
    /// <summary>
    /// Transfer order query response entity class
    /// </summary>
    public class TransferOrderQueryResModel : AgPayObject
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

        /// <summary>
        /// Payment interface code
        /// </summary>
        [JsonProperty("ifCode")]
        public string IfCode { get; set; }

        /// <summary>
        /// Entry method: WX_CASH-WeChat wallet; ALIPAY_CASH-Alipay transfer; BANK_CARD-Bank card
        /// </summary>
        [JsonProperty("entryType")]
        public string EntryType { get; set; }

        /// <summary>
        /// Transfer amount, unit: cents
        /// </summary>
        [JsonProperty("amount")]
        public long Amount { get; set; }

        /// <summary>
        /// Three-digit currency code, Chinese Yuan: CNY
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
        /// Transfer description
        /// </summary>
        [JsonProperty("transferDesc")]
        public string TransferDesc { get; set; }

        /// <summary>
        /// Payment status: 0-Order created, 1-Transferring, 2-Transfer successful, 3-Transfer failed, 4-Order closed
        /// </summary>
        [JsonProperty("state")]
        public byte State { get; set; }

        /// <summary>
        /// Specific channel additional parameters
        /// </summary>
        [JsonProperty("channelExtra")]
        public string ChannelExtra { get; set; }

        /// <summary>
        /// Channel order number
        /// </summary>
        [JsonProperty("channelOrderNo")]
        public string ChannelOrderNo { get; set; }

        /// <summary>
        /// Channel payment error code
        /// </summary>
        [JsonProperty("errCode")]
        public string ErrCode { get; set; }

        /// <summary>
        /// Channel payment error description
        /// </summary>
        [JsonProperty("errMsg")]
        public string ErrMsg { get; set; }

        /// <summary>
        /// Merchant extension parameters
        /// </summary>
        [JsonProperty("extParam")]
        public string ExtParam { get; set; }

        /// <summary>
        /// Transfer success time
        /// </summary>
        [JsonProperty("successTime")]
        public long SuccessTime { get; set; }

        /// <summary>
        /// Creation time
        /// </summary>
        [JsonProperty("createdAt")]
        public long CreatedAt { get; set; }
    }
}