using Zentin.Payment.Common.Constant;

namespace Zentin.Payment.ThirdPartyAdapters.RQRS.PayOrder.PayWay
{
    /// <summary>
    /// 支付方式： YSF_BAR
    /// </summary>
    public class YsfBarOrderRS : UnifiedOrderRS
    {
        public override string BuildPayDataType()
        {
            return Constants.PAY_DATA_TYPE.NONE;
        }

        public override string BuildPayData()
        {
            return "";
        }
    }
}
