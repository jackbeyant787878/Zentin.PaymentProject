using Zentin.Payment.Application.Contracts.Commands;

namespace Zentin.Payment.Application.Commands.AgentInfos
{
    public class RemoveAgentInfoCommand : CommandBase
    {
        /// <summary>
        /// 代理商号
        /// </summary>
        public string AgentNo { get; set; }

    }
}
