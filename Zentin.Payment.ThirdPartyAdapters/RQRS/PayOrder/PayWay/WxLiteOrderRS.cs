using Zentin.Payment.Common.Constant;

namespace Zentin.Payment.ThirdPartyAdapters.RQRS.PayOrder.PayWay
{
    /// <summary>
    /// 支付方式： WX_LITE
    /// </summary>
    public class WxLiteOrderRS : UnifiedOrderRS
    {
        /// <summary>
        /// 预支付数据包
        /// </summary>
        public string PayInfo { get; set; }

        public override string BuildPayDataType()
        {
            return Constants.PAY_DATA_TYPE.WX_APP;
        }

        public override string BuildPayData()
        {
            return PayInfo;
        }
    }
}
