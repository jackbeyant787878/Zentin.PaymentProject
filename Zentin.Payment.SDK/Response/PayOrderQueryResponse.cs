using Zentin.Payment.SDK.Models;

namespace Zentin.Payment.SDK.Response
{
    /// <summary>
    /// Payment inquiry response implementation
    /// </summary>
    public class PayOrderQueryResponse : AgPayResponse
    {
        public PayOrderQueryResModel Get()
        {
            if (Data == null) return new PayOrderQueryResModel();
            return Data.ToObject<PayOrderQueryResModel>();
        }
    }
}
