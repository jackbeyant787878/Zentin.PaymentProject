
using Zentin.Payment.Application.Contracts.Commands;

namespace Zentin.Payment.Application.Commands.MchInfos
{
    public class RemoveMchInfoCommand : CommandBase
    {
        /// <summary>
        /// 商户号
        /// </summary>
        public string MchNo { get; set; }


    }
}
