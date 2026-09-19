using Zentin.Payment.Common.Constant;

namespace Zentin.Payment.ThirdPartyAdapters.RQRS.PayOrder.PayWay
{
    public class WxBarOrderRS : UnifiedOrderRS
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
