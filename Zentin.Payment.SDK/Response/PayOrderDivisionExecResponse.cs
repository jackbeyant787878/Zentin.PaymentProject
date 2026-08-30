using Zentin.Payment.SDK.Models;

namespace Zentin.Payment.SDK.Response
{
    /// <summary>
    /// Initiate the split-fee response implementation
    /// </summary>
    public class PayOrderDivisionExecResponse : AgPayResponse
    {
        public PayOrderDivisionExecResModel Get()
        {
            if (Data == null) return new PayOrderDivisionExecResModel();
            return Data.ToObject<PayOrderDivisionExecResModel>();
        }

        public override bool IsSuccess(string signType, string apiKey)
        {
            if (base.IsSuccess(signType, apiKey))
            {
                int state = Get().State;
                return state == 1;
            }
            return false;
        }
    }
}
