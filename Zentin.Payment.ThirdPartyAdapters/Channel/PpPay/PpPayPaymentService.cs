using Microsoft.Extensions.Logging;
using Zentin.Payment.Application.Dto;
using Zentin.Payment.Common.Constant;
using Zentin.Payment.ThirdPartyAdapters.Models;
using Zentin.Payment.ThirdPartyAdapters.RQRS;
using Zentin.Payment.ThirdPartyAdapters.RQRS.PayOrder;
using Zentin.Payment.ThirdPartyAdapters.Services;
using Zentin.Payment.ThirdPartyAdapters.Utils;

namespace Zentin.Payment.ThirdPartyAdapters.Channel.PpPay
{
    public class PpPayPaymentService : AbstractPaymentService
    {
        public PpPayPaymentService(ILogger<PpPayPaymentService> logger,
            IServiceProvider serviceProvider,
            ISysConfigService sysConfigService,
            ConfigContextQueryService configContextQueryService)
            : base(logger, serviceProvider, sysConfigService, configContextQueryService)
        {
        }

        public PpPayPaymentService()
            : base()
        {
        }

        public override string GetIfCode()
        {
            return Constants.IF_CODE.PPPAY;
        }

        public override bool IsSupport(string wayCode)
        {
            return true;
        }

        public override Task<AbstractRS> PayAsync(UnifiedOrderRQ bizRQ, PayOrderDto payOrder, MchAppConfigContext mchAppConfigContext)
        {
            return PayWayUtil.GetRealPayWayService(this, payOrder.WayCode).PayAsync(bizRQ, payOrder, mchAppConfigContext);
        }

        public override Task<string> PreCheckAsync(UnifiedOrderRQ bizRQ, PayOrderDto payOrder, MchAppConfigContext mchAppConfigContext)
        {
            return PayWayUtil.GetRealPayWayService(this, payOrder.WayCode).PreCheckAsync(bizRQ, payOrder, mchAppConfigContext);
        }
    }
}
