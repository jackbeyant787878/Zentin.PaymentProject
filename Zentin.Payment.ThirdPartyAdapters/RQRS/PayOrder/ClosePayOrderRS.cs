using Zentin.Payment.ThirdPartyAdapters.RQRS.Msg;
using Newtonsoft.Json;

namespace Zentin.Payment.ThirdPartyAdapters.RQRS.PayOrder
{
    public class ClosePayOrderRS : AbstractRS
    {
        [JsonIgnore]
        public ChannelRetMsg ChannelRetMsg { get; set; }
    }
}
