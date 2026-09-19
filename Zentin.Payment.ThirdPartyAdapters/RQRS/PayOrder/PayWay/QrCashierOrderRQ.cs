using Zentin.Payment.Common.Constant;

namespace Zentin.Payment.ThirdPartyAdapters.RQRS.PayOrder.PayWay
{
    public class QrCashierOrderRQ : CommonPayDataRQ
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public QrCashierOrderRQ()
        {
            this.WayCode = Constants.PAY_WAY_CODE.QR_CASHIER;
        }
    }
}
