using Zentin.Payment.Application.Contracts.Commands;

namespace AGooday.AgPay.Domain.Commands.AgentInfos
{
    public class RemoveAgentInfoCommand : CommandBase
    {
        /// <summary>
        /// 代理商号
        /// </summary>
        public string AgentNo { get; set; }

    }
}
