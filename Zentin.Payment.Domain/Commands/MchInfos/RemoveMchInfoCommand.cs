
using Zentin.Payment.Application.Contracts.Commands;

namespace AGooday.AgPay.Domain.Commands.MchInfos
{
    public class RemoveMchInfoCommand : CommandBase
    {
        /// <summary>
        /// 商户号
        /// </summary>
        public string MchNo { get; set; }

     
    }
}
