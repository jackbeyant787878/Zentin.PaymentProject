using  Microsoft.Extensions.Logging;
using Zentin.Payment.Common.Constant;
using Zentin.Payment.ThirdPartyAdapters.Models;
using Zentin.Payment.ThirdPartyAdapters.RQRS.Msg;
using Zentin.Payment.ThirdPartyAdapters.Services;
using Zentin.Payment.ThirdPartyAdapters.Utils;
using static Zentin.Payment.ThirdPartyAdapters.Channel.IChannelRefundNoticeService;

namespace Zentin.Payment.ThirdPartyAdapters.Channel.UmsPay
{
    /// <summary>
    /// 银联商务 退款回调接口实现类
    /// </summary>
    public class UmsPayChannelRefundNoticeService : AbstractChannelRefundNoticeService
    {
        public UmsPayChannelRefundNoticeService(ILogger<AbstractChannelRefundNoticeService> logger,
            RequestKit requestKit,
            ConfigContextQueryService configContextQueryService)
            : base(logger, requestKit, configContextQueryService)
        {
        }

        public UmsPayChannelRefundNoticeService()
            : base()
        {
        }

        public override string GetIfCode()
        {
            return Constants.IF_CODE.UMSPAY;
        }

        public override Task<Dictionary<string, object>> ParseParamsAsync(HttpRequest request, string urlOrderId, NoticeTypeEnum noticeTypeEnum)
        {
            throw new NotImplementedException();
        }

        public override Task<ChannelRetMsg> DoNoticeAsync(HttpRequest request, object @params, RefundOrderDto payOrder, MchAppConfigContext mchAppConfigContext, NoticeTypeEnum noticeTypeEnum)
        {
            throw new NotImplementedException();
        }
    }
}
