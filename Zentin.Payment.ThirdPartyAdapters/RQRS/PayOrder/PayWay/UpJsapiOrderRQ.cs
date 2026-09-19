using Zentin.Payment.Common.Constant;

namespace Zentin.Payment.ThirdPartyAdapters.RQRS.PayOrder.PayWay
{
    /// <summary>
    /// 支付方式： UP_JSAPI
    /// </summary>
    public class UpJsapiOrderRQ : UnifiedOrderRQ
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UpJsapiOrderRQ()
        {
            this.WayCode = Constants.PAY_WAY_CODE.UP_JSAPI;
        }
    }
}
