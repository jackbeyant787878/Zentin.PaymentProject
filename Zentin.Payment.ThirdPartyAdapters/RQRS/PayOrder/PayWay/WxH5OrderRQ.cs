using Zentin.Payment.Common.Constant;

namespace Zentin.Payment.ThirdPartyAdapters.RQRS.PayOrder.PayWay
{
    /// <summary>
    /// 支付方式： WX_H5
    /// </summary>
    public class WxH5OrderRQ : CommonPayDataRQ
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WxH5OrderRQ()
        {
            this.WayCode = Constants.PAY_WAY_CODE.WX_H5;
        }
    }
}
