using Zentin.Payment.Common.Constant;

namespace Zentin.Payment.ThirdPartyAdapters.RQRS.PayOrder.PayWay
{
    /// <summary>
    /// 支付方式： ALI_APP
    /// </summary>
    public class AliAppOrderRS : UnifiedOrderRS
    {
        //private string PayData { get; set; }

        public override string BuildPayDataType()
        {
            return Constants.PAY_DATA_TYPE.ALI_APP;
        }

        public override string BuildPayData()
        {
            return PayData;
        }
    }
}
