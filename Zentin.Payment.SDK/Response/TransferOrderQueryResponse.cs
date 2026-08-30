using Zentin.Payment.SDK.Models;

namespace Zentin.Payment.SDK.Response
{
    /// <summary>
    /// Transferring funds inquiry response implementation
    /// </summary>
    public class TransferOrderQueryResponse : AgPayResponse
    {
        public TransferOrderQueryResModel Get()
        {
            if (Data == null) return new TransferOrderQueryResModel();
            return Data.ToObject<TransferOrderQueryResModel>();
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
