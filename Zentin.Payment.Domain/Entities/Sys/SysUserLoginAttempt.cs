using Zentin.Payment.Common;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Domain.Entities.Sys
{
    public sealed class SysUserLoginAttempt
    {
        public long Id { get; private set; }
        public long UserId { get; private set; }
        public IdentityType IdentityType { get; private set; }
        public string Identifier { get; private set; } = string.Empty;
        public string IpAddress { get; private set; } = string.Empty;
        public bool Success { get; private set; }
        public string SysType { get; private set; } = string.Empty;
        public DateTime AttemptTime { get; private set; }

        private SysUserLoginAttempt()
        {
            Id = SnowflakeIdGenerator.GetSnowflakeId();
        }

        public static SysUserLoginAttempt Record(
            long userId,
            IdentityType identityType,
            string identifier,
            string ipAddress,
            bool success,
            string sysType)
        {
            return new SysUserLoginAttempt
            {
                UserId = userId,
                IdentityType = identityType,
                Identifier = identifier,
                IpAddress = ipAddress,
                Success = success,
                SysType = sysType,
                AttemptTime = DateTime.UtcNow
            };
        }
    }
}