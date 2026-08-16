using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Domain.Entities.Device
{
    /// <summary>
    /// Device provider definition metadata
    /// </summary>
    public sealed class DeviceProviderDefine : BaseEntity
    {
        public string ProviderCode { get; private set; } = string.Empty;
        public string ProviderName { get; private set; } = string.Empty;
        public ConfigPageType ConfigPageType { get; private set; }
        public string? ProviderParams { get; private set; }
        public List<int> DeviceTypes { get; private set; } = [];
        public string? Icon { get; private set; }
        public string? BgColor { get; private set; }
        public bool State { get; private set; }
        public string? Remark { get; private set; }

        private DeviceProviderDefine() { }

        public static DeviceProviderDefine Create(
            string providerCode,
            string providerName,
            ConfigPageType configPageType,
            List<int> deviceTypes,
            string? icon = null,
            string? bgColor = null,
            string? remark = null,
            bool state = true)
        {
            if (string.IsNullOrWhiteSpace(providerCode))
                throw new ArgumentException("Provider code cannot be empty", nameof(providerCode));
            if (deviceTypes == null || deviceTypes.Count == 0)
                throw new ArgumentException("At least one device type is required", nameof(deviceTypes));

            return new DeviceProviderDefine
            {
                ProviderCode = providerCode,
                ProviderName = providerName,
                ConfigPageType = configPageType,
                DeviceTypes = deviceTypes,
                Icon = icon,
                BgColor = bgColor,
                Remark = remark,
                State = state
            };
        }

        public void UpdateParams(string providerParams)
        {
            ProviderParams = providerParams;
        }

        public void Enable() => State = true;
        public void Disable() => State = false;
    }
}
