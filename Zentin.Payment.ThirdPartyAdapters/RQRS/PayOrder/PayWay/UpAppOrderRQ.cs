using Zentin.Payment.Common.Constant;

namespace Zentin.Payment.ThirdPartyAdapters.RQRS.PayOrder.PayWay
{
    /// <summary>
    /// 支付方式： UPACP_APP
    /// </summary>
    public class UpAppOrderRQ : CommonPayDataRQ
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UpAppOrderRQ()
        {
            this.WayCode = Constants.PAY_WAY_CODE.UP_APP; //默认 wayCode, 避免validate出现问题
        }
    }
}
