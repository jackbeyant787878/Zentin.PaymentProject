using Zentin.Payment.SDK.Models;

namespace Zentin.Payment.SDK.Response
{
    /// <summary>
    /// Payment inquiry response implementation
    /// </summary>
    public class PayOrderCloseResponse : AgPayResponse
    {
        public PayOrderCloseResModel Get()
        {
            if (Data == null) return new PayOrderCloseResModel();
            return Data.ToObject<PayOrderCloseResModel>();
        }
    }
}
