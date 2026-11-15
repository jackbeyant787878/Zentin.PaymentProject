using Zentin.Payment.Common;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Domain.Entities.Sys
{
    public sealed class SysUserAuth
    {
        public long AuthId { get; private set; }
        public long UserId { get; private set; }
        public IdentityType IdentityType { get; private set; }
        public string Identifier { get; private set; } = string.Empty;
        public string Credential { get; private set; } = string.Empty;
        public string Salt { get; private set; } = string.Empty;
        public string SysType { get; private set; } = string.Empty;

        private SysUserAuth()
        {
            AuthId = SnowflakeIdGenerator.GetSnowflakeId();
        }

        public static SysUserAuth Create(
            long userId,
            IdentityType identityType,
            string identifier,
            string credential,
            string salt,
            string sysType)
        {
            if (userId <= 0)
                throw new ArgumentException("User id is required", nameof(userId));
            if (string.IsNullOrWhiteSpace(identifier))
                throw new ArgumentException("Identifier is required", nameof(identifier));
            if (string.IsNullOrWhiteSpace(credential))
                throw new ArgumentException("Credential is required", nameof(credential));

            return new SysUserAuth
            {
                UserId = userId,
                IdentityType = identityType,
                Identifier = identifier,
                Credential = credential,
                Salt = salt,
                SysType = sysType
            };
        }

        public void RotateCredential(string credential, string salt)
        {
            if (string.IsNullOrWhiteSpace(credential))
                throw new ArgumentException("Credential is required", nameof(credential));

            Credential = credential;
            Salt = salt;
        }
    }
}