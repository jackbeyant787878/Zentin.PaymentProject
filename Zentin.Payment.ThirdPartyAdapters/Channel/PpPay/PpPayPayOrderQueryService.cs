using Zentin.Payment.Application.Dto;
using Zentin.Payment.Common.Constant;
using Zentin.Payment.ThirdPartyAdapters.Models;
using Zentin.Payment.ThirdPartyAdapters.RQRS.Msg;
using Zentin.Payment.ThirdPartyAdapters.Services;

namespace Zentin.Payment.ThirdPartyAdapters.Channel.PpPay
{
    public class PpPayPayOrderQueryService : IPayOrderQueryService
    {
        private readonly ConfigContextQueryService _configContextQueryService;

        public PpPayPayOrderQueryService(ConfigContextQueryService configContextQueryService)
        {
            _configContextQueryService = configContextQueryService;
        }

        public PpPayPayOrderQueryService()
        {
        }

        public string GetIfCode()
        {
            return Constants.IF_CODE.PPPAY;
        }

        public async Task<ChannelRetMsg> QueryAsync(PayOrderDto payOrder, MchAppConfigContext mchAppConfigContext)
        {
            return await (await _configContextQueryService.GetPaypalWrapperAsync(mchAppConfigContext)).ProcessOrderAsync(null, payOrder);
        }
    }
}
