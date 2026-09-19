using Zentin.Payment.Application.Dto;
using Zentin.Payment.ThirdPartyAdapters.Models;
using Zentin.Payment.ThirdPartyAdapters.RQRS.Msg;

namespace Zentin.Payment.ThirdPartyAdapters.Channel
{
    /// <summary>
    /// 查单（渠道侧）接口定义
    /// </summary>
    public interface IPayOrderQueryService
    {
        /// <summary>
        /// 获取到接口code
        /// </summary>
        /// <returns></returns>
        string GetIfCode();

        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="payOrder"></param>
        /// <param name="mchAppConfigContext"></param>
        /// <returns></returns>
        Task<ChannelRetMsg> QueryAsync(PayOrderDto payOrder, MchAppConfigContext mchAppConfigContext);
    }
}
