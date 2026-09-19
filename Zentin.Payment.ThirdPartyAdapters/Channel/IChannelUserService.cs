using Zentin.Payment.ThirdPartyAdapters.Models;
using Newtonsoft.Json.Linq;

namespace Zentin.Payment.ThirdPartyAdapters.Channel
{
    /// <summary>
    /// 301方式获取渠道侧用户ID， 如微信openId 支付宝的userId等
    /// </summary>
    public interface IChannelUserService
    {
        /// <summary>
        /// 获取到接口code
        /// </summary>
        /// <returns></returns>
        string GetIfCode();

        /// <summary>
        /// 获取重定向地址
        /// </summary>
        /// <param name="callbackUrlEncode"></param>
        /// <param name="mchAppConfigContext"></param>
        /// <returns></returns>
        Task<string> BuildUserRedirectUrlAsync(string callbackUrlEncode, string wayCode, MchAppConfigContext mchAppConfigContext);

        /// <summary>
        /// 获取渠道用户ID
        /// </summary>
        /// <param name="reqParams"></param>
        /// <param name="mchAppConfigContext"></param>
        /// <returns></returns>
        Task<string> GetChannelUserIdAsync(JObject reqParams, string wayCode, MchAppConfigContext mchAppConfigContext);
    }
}
