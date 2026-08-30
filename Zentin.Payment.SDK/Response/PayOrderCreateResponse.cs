using Zentin.Payment.SDK.Models;

namespace Zentin.Payment.SDK.Response
{
    /// <summary>
    /// Payment order response implementation
    /// </summary>
    public class PayOrderCreateResponse : AgPayResponse
    {
        public PayOrderCreateResModel Get()
        {
            if (Data == null) return new PayOrderCreateResModel();
            return Data.ToObject<PayOrderCreateResModel>();
        }

        public override bool IsSuccess(string signType, string apiKey)
        {
            if (base.IsSuccess(signType, apiKey))
            {
                int orderState = Get().OrderState;
                return orderState == 0 || orderState == 1 || orderState == 2;
            }
            return false;
        }
    }
}
