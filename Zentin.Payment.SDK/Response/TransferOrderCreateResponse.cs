using Zentin.Payment.SDK.Models;

namespace Zentin.Payment.SDK.Response
{
    /// <summary>
    /// Transfer response implementation
    /// </summary>
    public class TransferOrderCreateResponse : AgPayResponse
    {
        public TransferOrderCreateResModel Get()
        {
            if (Data == null) return new TransferOrderCreateResModel();
            return Data.ToObject<TransferOrderCreateResModel>();
        }

        public override bool IsSuccess(string signType, string apiKey)
        {
            if (base.IsSuccess(signType, apiKey))
            {
                int state = Get().State;
                return state == 0 || state == 1 || state == 2;
            }
            return false;
        }
    }
}
