
namespace AGooday.AgPay.Domain.Commands.MchInfos
{
    public class ModifyMchInfoCommand : MchInfoCommand
    {
        /// <summary>
        /// 默认密码
        /// </summary>
        public bool DefaultPass { get; set; }

        /// <summary>
        /// 重置密码
        /// </summary>
        public bool ResetPass { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        public string ConfirmPwd { get; set; }

        /// <summary>
        /// 当前系统用户ID
        /// </summary>
        public long CurrentSysUserId { get; set; }

      
    }
}
