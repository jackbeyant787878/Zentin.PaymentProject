using Zentin.Payment.Common.Constant;

namespace Zentin.Payment.ThirdPartyAdapters.RQRS.PayOrder.PayWay
{
    /// <summary>
    /// 支付方式： ALI_WAP
    /// </summary>
    public class AliWapOrderRQ : CommonPayDataRQ
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AliWapOrderRQ()
        {
            this.WayCode = Constants.PAY_WAY_CODE.ALI_WAP;//默认 ALI_WAP, 避免validate出现问题
        }
    }
}
