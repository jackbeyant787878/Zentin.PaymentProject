using Zentin.Payment.Common.Constant;

namespace Zentin.Payment.ThirdPartyAdapters.RQRS.PayOrder.PayWay
{
    /// <summary>
    /// 支付方式： UPACP_QR
    /// </summary>
    public class UpQrOrderRQ : CommonPayDataRQ
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UpQrOrderRQ()
        {
            this.WayCode = Constants.PAY_WAY_CODE.UP_QR; //默认 wayCode, 避免validate出现问题
        }
    }
}
