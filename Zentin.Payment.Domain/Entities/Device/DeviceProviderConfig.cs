using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Common;
using Zentin.Payment.Domain.Abstractions;
using static Zentin.Payment.Domain.Enums.Enums;

namespace Zentin.Payment.Domain.Entities.Device
{
    /// <summary>
    /// Device provider runtime configuration
    /// </summary>
    public sealed class DeviceProviderConfig : BaseEntity
    {
        public long DeviceProviderConfigId { get; private set; }
        public DeviceType DeviceType { get; private set; }
        public string ProviderCode { get; private set; } = string.Empty;
        public string ProviderParams { get; private set; } = string.Empty;
        public bool State { get; private set; }
        public string? Remark { get; private set; }

        private DeviceProviderConfig()
        {
            DeviceProviderConfigId = SnowflakeIdGenerator.GetSnowflakeId();
        }

        public static DeviceProviderConfig Create(DeviceType deviceType,string providerCode,string providerParams,string? remark = null,bool state = true)
        {
            if (string.IsNullOrWhiteSpace(providerCode))
                throw new ArgumentException("Provider code cannot be empty", nameof(providerCode));
            if (string.IsNullOrWhiteSpace(providerParams))
                throw new ArgumentException("Provider params cannot be empty", nameof(providerParams));

            return new DeviceProviderConfig
            {
                DeviceType = deviceType,
                ProviderCode = providerCode,
                ProviderParams = providerParams,
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
