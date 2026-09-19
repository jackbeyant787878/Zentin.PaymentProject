using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Zentin.Payment.Common.Constant;
using Zentin.Payment.Common.Exceptions;
using Zentin.Payment.ThirdPartyAdapters.Models;
using Zentin.Payment.ThirdPartyAdapters.RQRS.Msg;
using Zentin.Payment.ThirdPartyAdapters.Services;
using Zentin.Payment.ThirdPartyAdapters.Utils;
using static Zentin.Payment.ThirdPartyAdapters.Channel.IChannelRefundNoticeService;

namespace Zentin.Payment.ThirdPartyAdapters.Channel.PpPay
{
    public class PpPayChannelRefundNoticeService : AbstractChannelRefundNoticeService
    {
        public PpPayChannelRefundNoticeService(ILogger<PpPayChannelRefundNoticeService> logger,
            RequestKit requestKit,
            ConfigContextQueryService configContextQueryService)
            : base(logger, requestKit, configContextQueryService)
        {
        }

        public PpPayChannelRefundNoticeService()
            : base()
        {
        }

        public override string GetIfCode()
        {
            return Constants.IF_CODE.PPPAY;
        }

        public override async Task<Dictionary<string, object>> ParseParamsAsync(HttpRequest request, string urlOrderId, NoticeTypeEnum noticeTypeEnum)
        {
            JObject paramsObj = await GetReqParamJsonAsync();
            // 获取退款订单 Paypal ID
            string orderId = paramsObj.SelectToken("resource.invoice_id")?.ToString();
            return new Dictionary<string, object>() { { orderId, paramsObj } };
        }

        public override async Task<ChannelRetMsg> DoNoticeAsync(HttpRequest request, object @params, RefundOrderDto payOrder, MchAppConfigContext mchAppConfigContext, NoticeTypeEnum noticeTypeEnum)
        {
            try
            {
                JObject obj = (JObject)@params;
                string orderId = obj.SelectToken("resource.id")?.ToString();

                PayPalWrapper wrapper = mchAppConfigContext.GetPaypalWrapper();

                // 查询退款详情以及状态
                RefundsGetInput refundRequest = new RefundsGetInput(orderId);
                var response = await wrapper.Client.PaymentsController.RefundsGetAsync(refundRequest);

                ChannelRetMsg channelRetMsg = ChannelRetMsg.Waiting();
                channelRetMsg.ResponseEntity = PayPalWrapper.TextResp("ERROR");

                if ((int)response.StatusCode == 200)
                {
                    string responseJson = JsonConvert.SerializeObject(response.Data);
                    channelRetMsg = PayPalWrapper.DispatchCode(response.Data.Status.Value, channelRetMsg);
                    channelRetMsg.ChannelAttach = responseJson;
                    channelRetMsg.ChannelOrderId = response.Data.Id;
                    channelRetMsg.ResponseEntity = PayPalWrapper.TextResp("SUCCESS");
                }
                else
                {
                    channelRetMsg.ChannelState = ChannelState.CONFIRM_FAIL;
                    channelRetMsg.ChannelErrCode = "201";
                    channelRetMsg.ChannelErrMsg = "异步退款失败，Paypal 响应非 200";
                }

                return channelRetMsg;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "error");
                throw ResponseException.BuildText("ERROR");
            }
        }
    }
}
