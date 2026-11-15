using Zentin.Payment.Common;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Domain.Entities.Sys
{
    public sealed class SysUser : AggregateRoot
    {
        public long SysUserId { get; private set; }
        public string LoginUsername { get; private set; } = string.Empty;
        public string Realname { get; private set; } = string.Empty;
        public string Telphone { get; private set; } = string.Empty;
        public Sex Sex { get; private set; }
        public string? AvatarUrl { get; private set; }
        public string? UserNo { get; private set; }
        public string? SafeWord { get; private set; }
        public bool InitUser { get; private set; }
        public SysUserType UserType { get; private set; }
        public string? EntRules { get; private set; }
        public string? BindStoreIds { get; private set; }
        public string? InviteCode { get; private set; }
        public long? TeamId { get; private set; }
        public byte? IsTeamLeader { get; private set; }
        public SysUserState State { get; private set; }
        public string SysType { get; private set; } = string.Empty;
        public string BelongInfoId { get; private set; } = string.Empty;

        private SysUser()
        {
            SysUserId = SnowflakeIdGenerator.GetSnowflakeId();
        }

        public static SysUser Create(
            string loginUsername,
            string realname,
            string telphone,
            SysUserType userType,
            string sysType,
            string belongInfoId,
            Sex sex = Sex.Unknown,
            bool initUser = false,
            string? userNo = null,
            string? inviteCode = null)
        {
            if (string.IsNullOrWhiteSpace(loginUsername))
                throw new ArgumentException("Login username is required", nameof(loginUsername));
            if (string.IsNullOrWhiteSpace(realname))
                throw new ArgumentException("Realname is required", nameof(realname));
            if (string.IsNullOrWhiteSpace(sysType))
                throw new ArgumentException("System type is required", nameof(sysType));
            if (string.IsNullOrWhiteSpace(belongInfoId))
                throw new ArgumentException("Belong info id is required", nameof(belongInfoId));

            return new SysUser
            {
                LoginUsername = loginUsername,
                Realname = realname,
                Telphone = telphone,
                Sex = sex,
                InitUser = initUser,
                UserType = userType,
                UserNo = userNo,
                InviteCode = inviteCode,
                State = SysUserState.Normal,
                SysType = sysType,
                BelongInfoId = belongInfoId
            };
        }

        public void Enable()
        {
            if (State == SysUserState.Normal)
                throw new InvalidOperationException("User is already enabled");

            State = SysUserState.Normal;
        }

        public void Disable()
        {
            if (State == SysUserState.Disabled)
                throw new InvalidOperationException("User is already disabled");
            if (InitUser)
                throw new InvalidOperationException("The initial user cannot be disabled");

            State = SysUserState.Disabled;
        }

        public void BindTeam(long teamId, bool isTeamLeader)
        {
            TeamId = teamId;
            IsTeamLeader = (byte)(isTeamLeader ? 1 : 0);
        }

        public void UpdateProfile(string realname, string telphone, Sex sex, string? avatarUrl)
        {
            if (string.IsNullOrWhiteSpace(realname))
                throw new ArgumentException("Realname is required", nameof(realname));

            Realname = realname;
            Telphone = telphone;
            Sex = sex;
            AvatarUrl = avatarUrl;
        }
    }
}