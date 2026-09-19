using System.ComponentModel.DataAnnotations;

namespace Zentin.Payment.ThirdPartyAdapters.RQRS
{
    /// <summary>
    /// 通用RQ, 包含mchNo和appId 必填项
    /// </summary>
    public abstract class AbstractMchAppRQ : AbstractRQ
    {
        /// <summary>
        /// 商户号
        /// </summary>
        [Required(ErrorMessage = "商户号不能为空")]
        public string MchNo { get; set; }

        /// <summary>
        /// 商户应用ID
        /// </summary>
        [Required(ErrorMessage = "商户应用ID不能为空")]
        public string AppId { get; set; }
    }
}
