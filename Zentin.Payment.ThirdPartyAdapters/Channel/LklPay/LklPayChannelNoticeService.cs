using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Zentin.Payment.Application.Dto;
using Zentin.Payment.Application.LklPay;
using Zentin.Payment.Common.Constant;
using Zentin.Payment.Common.Exceptions;
using Zentin.Payment.Common.Utils;
using Zentin.Payment.ThirdPartyAdapters.Channel.LklPay.Enumerator;
using Zentin.Payment.ThirdPartyAdapters.Channel.LklPay.Utils;
using Zentin.Payment.ThirdPartyAdapters.Models;
using Zentin.Payment.ThirdPartyAdapters.RQRS.Msg;
using Zentin.Payment.ThirdPartyAdapters.Services;
using Zentin.Payment.ThirdPartyAdapters.Utils;
using static Zentin.Payment.ThirdPartyAdapters.Channel.IChannelNoticeService;

namespace Zentin.Payment.ThirdPartyAdapters.Channel.LklPay
{
    /// <summary>
    /// 拉卡拉回调
    /// </summary>
    public class LklPayChannelNoticeService : AbstractChannelNoticeService
    {
        public LklPayChannelNoticeService(ILogger<LklPayChannelNoticeService> logger,
            RequestKit requestKit,
            ConfigContextQueryService configContextQueryService)
            : base(logger, requestKit, configContextQueryService)
        {
        }

        public LklPayChannelNoticeService()
            : base()
        {
        }

        public override string GetIfCode()
        {
            return Constants.IF_CODE.LKLPAY;
        }

        public override async Task<Dictionary<string, object>> ParseParamsAsync(HttpRequest request, string urlOrderId, NoticeTypeEnum noticeTypeEnum)
        {
            try
            {
                JObject @params = await GetReqParamJsonAsync();
                string payOrderId = @params.GetValue("out_trade_no").ToString();
                return new Dictionary<string, object>() { { payOrderId, @params } };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "error");
                throw ResponseException.BuildText("ERROR");
            }
        }

        public override async Task<ChannelRetMsg> DoNoticeAsync(HttpRequest request, object @params, PayOrderDto payOrder, MchAppConfigContext mchAppConfigContext, NoticeTypeEnum noticeTypeEnum)
        {
            try
            {
                ChannelRetMsg result = ChannelRetMsg.ConfirmSuccess(null);

                string logPrefix = "【处理拉卡拉支付回调】";
                // 获取请求参数
                JObject jsonParams = JObject.FromObject(@params);
                _logger.LogInformation("{logPrefix} 回调参数, 报文: {jsonParams}", logPrefix, jsonParams);
                //_logger.LogInformation($"{logPrefix} 回调参数, 报文: {jsonParams}");

                // 校验支付回调
                bool verifyResult = await VerifyParamsAsync(request, jsonParams, payOrder, mchAppConfigContext);
                // 验证参数失败
                if (!verifyResult)
                {
                    throw ResponseException.BuildText("ERROR");
                }
                _logger.LogInformation("{logPrefix} 验证支付通知数据及签名通过", logPrefix);
                //_logger.LogInformation($"{logPrefix} 验证支付通知数据及签名通过");

                //验签成功后判断上游订单状态
                JObject resJSON = new JObject();
                resJSON.Add("code", "SUCCESS");
                resJSON.Add("message", "执行成功");
                var okResponse = JsonResp(resJSON);
                result.ResponseEntity = okResponse;

                jsonParams.TryGetString("merchant_no", out string merchantNo);
                string tradeState = jsonParams.GetValue("trade_state").ToString();
                jsonParams.TryGetString("trade_state_desc", out string tradeStateDesc);
                string tradeNo = jsonParams.GetValue("trade_no").ToString();//拉卡拉商户订单号
                string accTradeNo = jsonParams.GetValue("acc_trade_no").ToString();//拉卡拉商户订单号
                jsonParams.TryGetString("user_id1", out string userId1);
                jsonParams.TryGetString("user_id2", out string userId2);
                var orderStatus = LklPayEnum.ConvertTradeState(tradeState);
                switch (orderStatus)
                {
                    case LklPayEnum.TradeState.SUCCESS:
                        result.ChannelMchNo = merchantNo;
                        result.ChannelOrderId = tradeNo;
                        result.ChannelUserId = userId2 ?? userId1;
                        result.PlatformOrderId = accTradeNo;
                        result.PlatformMchOrderId = tradeNo;
                        result.ChannelState = ChannelState.CONFIRM_SUCCESS;
                        break;
                    case LklPayEnum.TradeState.FAIL:
                        result.ChannelState = ChannelState.CONFIRM_FAIL;
                        result.ChannelErrMsg = tradeStateDesc;
                        break;
                }
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "error");
                throw ResponseException.BuildText("ERROR");
            }
        }

        public async Task<bool> VerifyParamsAsync(HttpRequest request, JObject jsonParams, PayOrderDto payOrder, MchAppConfigContext mchAppConfigContext)
        {
            string ordNo = jsonParams.GetValue("out_trade_no").ToString();       // 商户订单号
            string amt = jsonParams.GetValue("total_amount").ToString();         // 支付金额
            if (string.IsNullOrWhiteSpace(ordNo))
            {
                _logger.LogInformation("订单ID为空 [orderNo]={ordNo}", ordNo);
                //_logger.LogInformation($"订单ID为空 [orderNo]={ordNo}");
                return false;
            }
            if (string.IsNullOrWhiteSpace(amt))
            {
                _logger.LogInformation("金额参数为空 [amt]={amt}", amt);
                //_logger.LogInformation($"金额参数为空 [amt]={amt}");
                return false;
            }

            LklPayIsvParams isvParams = (LklPayIsvParams)await _configContextQueryService.QueryIsvParamsAsync(mchAppConfigContext.MchInfo.IsvNo, GetIfCode());

            //验签
            string publicKey = isvParams.PublicCert;

            //验签失败
            var headers = request.Headers.ToDictionary(h => h.Key, h => h.Value.FirstOrDefault());
            if (!LklPaySignUtil.NoticeVerify(headers, jsonParams.ToString(), publicKey))
            {
                _logger.LogInformation("【拉卡拉回调】 验签失败！ 回调参数: parameter={jsonParams}, publicKey={publicKey} ", jsonParams, publicKey);
                //_logger.LogInformation($"【拉卡拉回调】 验签失败！ 回调参数: parameter={jsonParams}, publicKey={publicKey} ");
                return false;
            }

            // 核对金额
            long dbPayAmt = payOrder.Amount;
            if (dbPayAmt != Convert.ToInt64(amt))
            {
                _logger.LogInformation("订单金额与参数金额不符。 dbPayAmt={dbPayAmt}, amt={amt}, payOrderId={ordNo}", dbPayAmt, amt, ordNo);
                //_logger.LogInformation($"订单金额与参数金额不符。 dbPayAmt={dbPayAmt}, amt={amt}, payOrderId={ordNo}");
                return false;
            }
            return true;
        }
    }
}
