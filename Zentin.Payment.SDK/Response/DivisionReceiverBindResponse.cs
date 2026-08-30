using Zentin.Payment.SDK.Models;

namespace Zentin.Payment.SDK.Response
{
    /// <summary>
    /// The implementation of the response for binding the accounting account
    /// </summary>
    public class DivisionReceiverBindResponse : AgPayResponse
    {
        public DivisionReceiverBindResModel Get()
        {
            if (Data == null) return new DivisionReceiverBindResModel();
            return Data.ToObject<DivisionReceiverBindResModel>();
        }

        public override bool IsSuccess(string signType, string apiKey)
        {
            if (base.IsSuccess(signType, apiKey))
            {
                int state = Get().BindState;
                return state == 1;
            }
            return false;
        }
    }
}
