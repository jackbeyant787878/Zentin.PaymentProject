using Newtonsoft.Json;

namespace Zentin.Payment.SDK.Models
{
    /// <summary>
    /// Response result for profit sharing receiver binding
    /// </summary>
    public class DivisionReceiverBindResModel : AgPayObject
    {
        /// <summary>
        /// Sharing receiver ID
        /// </summary>
        [JsonProperty("receiverId")]
        public long ReceiverId { get; set; }

        /// <summary>
        /// Receiver account alias
        /// </summary>
        [JsonProperty("receiverAlias")]
        public string ReceiverAlias { get; set; }

        /// <summary>
        /// Group ID (for merchant interface usage)
        /// </summary>
        [JsonProperty("receiverGroupId")]
        public long ReceiverGroupId { get; set; }

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
        /// Payment interface code
        /// </summary>
        [JsonProperty("ifCode")]
        public string IfCode { get; set; }

        /// <summary>
        /// Sharing receiver account type: 0-Individual(private) 1-Merchant(public)
        /// </summary>
        [JsonProperty("accType")]
        public byte AccType { get; set; }

        /// <summary>
        /// Sharing receiver account number
        /// </summary>
        [JsonProperty("accNo")]
        public string AccNo { get; set; }

        /// <summary>
        /// Sharing receiver account name
        /// </summary>
        [JsonProperty("accName")]
        public string AccName { get; set; }

        /// <summary>
        /// Sharing relationship type (refer to WeChat), e.g.: SERVICE_PROVIDER service provider, etc.
        /// </summary>
        [JsonProperty("relationType")]
        public string RelationType { get; set; }

        /// <summary>
        /// When custom selection is made, this field needs to be entered. Otherwise it will be the corresponding name
        /// </summary>
        [JsonProperty("relationTypeName")]
        public string RelationTypeName { get; set; }

        /// <summary>
        /// Channel specific information
        /// </summary>
        [JsonProperty("channelExtInfo")]
        public string ChannelExtInfo { get; set; }

        /// <summary>
        /// Binding success time
        /// </summary>
        [JsonProperty("bindSuccessTime")]
        public string BindSuccessTime { get; set; }

        /// <summary>
        /// Profit sharing ratio
        /// </summary>
        [JsonProperty("divisionProfit")]
        public string DivisionProfit { get; set; }

        /// <summary>
        /// Sharing status 1-Binding successful, 0-Binding exception
        /// </summary>
        [JsonProperty("bindState")]
        public byte BindState { get; set; }

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