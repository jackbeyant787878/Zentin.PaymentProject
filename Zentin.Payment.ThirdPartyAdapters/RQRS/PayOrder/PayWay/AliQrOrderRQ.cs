using Zentin.Payment.Common.Constant;

namespace Zentin.Payment.ThirdPartyAdapters.RQRS.PayOrder.PayWay
{
    /// <summary>
    /// 支付方式： ALI_QR
    /// </summary>
    public class AliQrOrderRQ : CommonPayDataRQ
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AliQrOrderRQ()
        {
            this.WayCode = Constants.PAY_WAY_CODE.ALI_QR;
        }
    }
}
