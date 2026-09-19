using Zentin.Payment.Common.Constant;
using Zentin.Payment.Common.Utils;

namespace Zentin.Payment.ThirdPartyAdapters.RQRS.PayOrder.PayWay
{
    /// <summary>
    /// 支付方式： ALI_JSAPI
    /// </summary>
    public class AliJsapiOrderRS : UnifiedOrderRS
    {
        public string AlipayTradeNo { get; set; }

        public override string BuildPayDataType()
        {
            return Constants.PAY_DATA_TYPE.ALI_APP;
        }

        public override string BuildPayData()
        {
            return JsonUtil.NewJson("alipayTradeNo", AlipayTradeNo).ToString();
        }
    }
}
