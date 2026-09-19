using Zentin.Payment.Common.Constant;

namespace Zentin.Payment.ThirdPartyAdapters.RQRS.PayOrder.PayWay
{
    /// <summary>
    /// 支付方式： UPACP_PC
    /// </summary>
    public class UpPcOrderRQ : CommonPayDataRQ
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UpPcOrderRQ()
        {
            this.WayCode = Constants.PAY_WAY_CODE.UP_PC; //默认 wayCode, 避免validate出现问题
        }
    }
}
