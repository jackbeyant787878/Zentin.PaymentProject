using System.ComponentModel.DataAnnotations;
using Zentin.Payment.Common.Constant;

namespace Zentin.Payment.ThirdPartyAdapters.RQRS.PayOrder.PayWay
{
    /// <summary>
    /// 支付方式： PP_PC
    /// </summary>
    public class PpPcOrderRQ : CommonPayDataRQ
    {
        [Required(ErrorMessage = "取消支付返回站点")]
        public string CancelUrl { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PpPcOrderRQ()
        {
            this.WayCode = Constants.PAY_WAY_CODE.PP_PC;
        }
    }
}
