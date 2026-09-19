using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Zentin.Payment.Application.Dto;
using Zentin.Payment.Common.Constant;
using Zentin.Payment.ThirdPartyAdapters.Channel.AliPay.Kits;
using Zentin.Payment.ThirdPartyAdapters.Models;
using Zentin.Payment.ThirdPartyAdapters.RQRS.Msg;
using Zentin.Payment.ThirdPartyAdapters.Services;

namespace Zentin.Payment.ThirdPartyAdapters.Channel.AliPay
{
    /// <summary>
    /// 支付宝 关闭订单接口实现类
    /// </summary>
    public class AliPayPayOrderCloseService : IPayOrderCloseService
    {
        private readonly ConfigContextQueryService _configContextQueryService;

        public AliPayPayOrderCloseService(ConfigContextQueryService configContextQueryService)
        {
            _configContextQueryService = configContextQueryService;
        }

        public AliPayPayOrderCloseService()
        {
        }

        public string GetIfCode()
        {
            return Constants.IF_CODE.ALIPAY;
        }

        public async Task<ChannelRetMsg> CloseAsync(PayOrderDto payOrder, MchAppConfigContext mchAppConfigContext)
        {
            AlipayTradeCloseRequest req = new AlipayTradeCloseRequest();

            // 商户订单号，商户网站订单系统中唯一订单号，必填
            AlipayTradeCloseModel model = new AlipayTradeCloseModel();
            model.OutTradeNo = payOrder.PayOrderId;
            req.SetBizModel(model);

            //通用字段
            await AliPayKit.PutApiIsvInfoAsync(mchAppConfigContext, req, model);

            AlipayTradeCloseResponse resp = (await _configContextQueryService.GetAlipayClientWrapperAsync(mchAppConfigContext)).Execute(req);

            // 返回状态成功
            if (!resp.IsError)
            {
                return ChannelRetMsg.ConfirmSuccess(resp.TradeNo);
            }
            else
            {
                return ChannelRetMsg.SysError(resp.SubMsg);
            }
        }
    }
}
