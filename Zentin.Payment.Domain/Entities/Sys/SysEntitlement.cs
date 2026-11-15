using Zentin.Payment.Domain.Abstractions;

namespace Zentin.Payment.Domain.Entities.Sys
{
    public sealed class SysEntitlement : AggregateRoot
    {
        public string EntId { get; private set; } = string.Empty;
        public string EntName { get; private set; } = string.Empty;
        public string? MenuIcon { get; private set; }
        public string? MenuUri { get; private set; }
        public string? ComponentName { get; private set; }
        public string EntType { get; private set; } = string.Empty;
        public byte QuickJump { get; private set; }
        public string? MatchRule { get; private set; }
        public byte State { get; private set; }
        public string Pid { get; private set; } = string.Empty;
        public int EntSort { get; private set; }
        public string SysType { get; private set; } = string.Empty;

        private SysEntitlement() { }

        public static SysEntitlement Create(
            string entId,
            string entName,
            string entType,
            string sysType,
            string pid = "0",
            int entSort = 0,
            string? menuIcon = null,
            string? menuUri = null,
            string? componentName = null,
            string? matchRule = null)
        {
            if (string.IsNullOrWhiteSpace(entId))
                throw new ArgumentException("Entitlement id is required", nameof(entId));
            if (string.IsNullOrWhiteSpace(entName))
                throw new ArgumentException("Entitlement name is required", nameof(entName));
            if (entType is not ("ML" or "MO" or "PB"))
                throw new ArgumentException("Entitlement type must be ML, MO or PB", nameof(entType));

            return new SysEntitlement
            {
                EntId = entId,
                EntName = entName,
                EntType = entType,
                SysType = sysType,
                Pid = pid,
                EntSort = entSort,
                MenuIcon = menuIcon,
                MenuUri = menuUri,
                ComponentName = componentName,
                MatchRule = matchRule,
                QuickJump = 0,
                State = 1
            };
        }

        public void Enable() => State = 1;

        public void Disable() => State = 0;

        public void Reorder(int entSort) => EntSort = entSort;
    }
}