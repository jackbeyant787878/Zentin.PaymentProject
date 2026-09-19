using Zentin.Payment.Common.Constant;
using Zentin.Payment.Common.Utils;

namespace Zentin.Payment.ThirdPartyAdapters.RQRS.PayOrder.PayWay
{
    /// <summary>
    /// 支付方式： UP_JSAPI
    /// </summary>
    public class UpJsapiOrderRS : UnifiedOrderRS
    {
        /// <summary>
        /// 调起支付插件的云闪付订单号
        /// </summary>
        public string RedirectUrl { get; set; }

        public override string BuildPayDataType()
        {
            return Constants.PAY_DATA_TYPE.YSF_APP;
        }

        public override string BuildPayData()
        {
            return JsonUtil.NewJson("redirectUrl", RedirectUrl).ToString();
        }
    }
}
