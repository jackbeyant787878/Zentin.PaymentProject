using Newtonsoft.Json;

namespace Zentin.Payment.SDK.Models
{
    /// <summary>
    /// Execute profit sharing
    /// </summary>
    public class PayOrderDivisionExecReqModel : AgPayObject
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
        /// Whether to use the system-configured automatic profit sharing group: 0-No 1-Yes
        /// </summary>
        [JsonProperty("useSysAutoDivisionReceivers")]
        public byte UseSysAutoDivisionReceivers { get; set; }

        /// <summary>
        /// Receiver account list (JSONArray converted to string type)
        /// Only valid when useSysAutoDivisionReceivers=0.
        /// 
        /// Reference:
        /// 
        /// Method 1: By account dimension
        /// [{
        ///     receiverId: 800001,
        ///     divisionProfit: 0.1 (If not filled in, use system default value)
        /// }]
        /// 
        /// Method 2: By group dimension
        /// [{
        ///     receiverGroupId: 100001, (All channel accounts in the group that belong to the current order and are in available status participate in the sharing)
        ///     divisionProfit: 0.1 (Each account's sharing ratio, if not filled in, use system default value, it is recommended to leave blank)
        /// }]
        /// </summary>
        [JsonProperty("receivers")]
        public string Receivers { get; set; }
    }
}