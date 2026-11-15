using Zentin.Payment.Domain.Abstractions;

namespace Zentin.Payment.Domain.Entities.Sys
{
    public sealed class SysConfig : AggregateRoot
    {
        public string ConfigKey { get; private set; } = string.Empty;
        public string ConfigName { get; private set; } = string.Empty;
        public string ConfigDesc { get; private set; } = string.Empty;
        public string GroupKey { get; private set; } = string.Empty;
        public string GroupName { get; private set; } = string.Empty;
        public string ConfigVal { get; private set; } = string.Empty;
        public string Type { get; private set; } = string.Empty;
        public string SysType { get; private set; } = string.Empty;
        public string BelongInfoId { get; private set; } = string.Empty;
        public long SortNum { get; private set; }

        private SysConfig() { }

        public static SysConfig Create(
            string configKey,
            string configName,
            string groupKey,
            string groupName,
            string type,
            string sysType,
            string belongInfoId,
            string configVal = "",
            string configDesc = "",
            long sortNum = 0)
        {
            if (string.IsNullOrWhiteSpace(configKey))
                throw new ArgumentException("Config key is required", nameof(configKey));
            if (string.IsNullOrWhiteSpace(sysType))
                throw new ArgumentException("System type is required", nameof(sysType));
            if (string.IsNullOrWhiteSpace(belongInfoId))
                throw new ArgumentException("Belong info id is required", nameof(belongInfoId));

            return new SysConfig
            {
                ConfigKey = configKey,
                ConfigName = configName,
                ConfigDesc = configDesc,
                GroupKey = groupKey,
                GroupName = groupName,
                ConfigVal = configVal,
                Type = type,
                SysType = sysType,
                BelongInfoId = belongInfoId,
                SortNum = sortNum
            };
        }

        public void SetValue(string configVal) => ConfigVal = configVal;

        public void Reorder(long sortNum) => SortNum = sortNum;
    }
}