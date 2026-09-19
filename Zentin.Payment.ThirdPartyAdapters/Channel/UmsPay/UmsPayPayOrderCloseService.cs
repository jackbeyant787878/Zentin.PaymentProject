using  Microsoft.Extensions.Logging;
using Zentin.Payment.Common.Constant;
using Zentin.Payment.ThirdPartyAdapters.Models;
using Zentin.Payment.ThirdPartyAdapters.RQRS.Msg;

namespace Zentin.Payment.ThirdPartyAdapters.Channel.UmsPay
{
    /// <summary>
    /// 银联商务关单
    /// </summary>
    public class UmsPayPayOrderCloseService : IPayOrderCloseService
    {
        public UmsPayPayOrderCloseService()
        {
        }

        public string GetIfCode()
        {
            return Constants.IF_CODE.UMSPAY;
        }
        public Task<ChannelRetMsg> CloseAsync(PayOrderDto payOrder, MchAppConfigContext mchAppConfigContext)
        {
            throw new NotImplementedException();
        }
    }
}
