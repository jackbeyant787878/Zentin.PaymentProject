using Zentin.Payment.Application.Params;

namespace Zentin.Payment.Application.YsfPay
{
    /// <summary>
    /// 云闪付 配置信息
    /// </summary>
    public class YsfPayIsvSubMchParams : IsvSubMchParams
    {
        /// <summary>
        /// 商户编号
        /// </summary>
        public string MerId { get; set; }
    }
}
