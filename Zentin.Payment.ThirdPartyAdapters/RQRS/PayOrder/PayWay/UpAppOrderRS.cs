using Zentin.Payment.Common.Constant;

namespace Zentin.Payment.ThirdPartyAdapters.RQRS.PayOrder.PayWay
{
    /// <summary>
    /// 支付方式： UPACP_APP
    /// </summary>
    public class UpAppOrderRS : CommonPayDataRS
    {
        //private string PayData { get; set; }

        public override string BuildPayDataType()
        {
            return Constants.PAY_DATA_TYPE.YSF_APP;
        }

        public override string BuildPayData()
        {
            return PayData;
        }
    }
}
