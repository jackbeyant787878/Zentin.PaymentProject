using Zentin.Payment.SDK.Models;

namespace Zentin.Payment.SDK.Response
{
    /// <summary>
    /// Refund Inquiry Response Implementation
    /// </summary>
    public class RefundOrderQueryResponse : AgPayResponse
    {
        public RefundOrderQueryResModel Get()
        {
            if (Data == null) return new RefundOrderQueryResModel();
            return Data.ToObject<RefundOrderQueryResModel>();
        }
    }
}
