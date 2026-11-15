using Zentin.Payment.Common;
using Zentin.Payment.Domain.Abstractions;

namespace Zentin.Payment.Domain.Entities.Sys
{
    public sealed class SysUserTeam : AggregateRoot
    {
        public long TeamId { get; private set; }
        public string TeamName { get; private set; } = string.Empty;
        public string TeamNo { get; private set; } = string.Empty;
        public string StatRangeType { get; private set; } = string.Empty;
        public string SysType { get; private set; } = string.Empty;
        public string BelongInfoId { get; private set; } = string.Empty;
        public long? CreatedUid { get; private set; }
        public string? CreatedBy { get; private set; }

        private SysUserTeam()
        {
            TeamId = SnowflakeIdGenerator.GetSnowflakeId();
        }

        public static SysUserTeam Create(
            string teamName,
            string teamNo,
            string statRangeType,
            string sysType,
            string belongInfoId,
            long? createdUid = null,
            string? createdBy = null)
        {
            if (string.IsNullOrWhiteSpace(teamName))
                throw new ArgumentException("Team name is required", nameof(teamName));
            if (string.IsNullOrWhiteSpace(teamNo))
                throw new ArgumentException("Team number is required", nameof(teamNo));

            return new SysUserTeam
            {
                TeamName = teamName,
                TeamNo = teamNo,
                StatRangeType = statRangeType,
                SysType = sysType,
                BelongInfoId = belongInfoId,
                CreatedUid = createdUid,
                CreatedBy = createdBy
            };
        }

        public void Rename(string teamName)
        {
            if (string.IsNullOrWhiteSpace(teamName))
                throw new ArgumentException("Team name is required", nameof(teamName));

            TeamName = teamName;
        }

        public void ChangeStatRange(string statRangeType)
        {
            if (string.IsNullOrWhiteSpace(statRangeType))
                throw new ArgumentException("Stat range type is required", nameof(statRangeType));

            StatRangeType = statRangeType;
        }
    }
}