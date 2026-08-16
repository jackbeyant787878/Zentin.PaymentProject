using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Common;
using Zentin.Payment.Domain.Abstractions;
using static Zentin.Payment.Domain.Enums.Enums;

namespace Zentin.Payment.Domain.Entities.Device
{
    /// <summary>
    /// Physical device information
    /// </summary>
    public sealed class DeviceInfo : BaseEntity
    {
        public long DeviceInfoId { get; private set; }
        public string DeviceNo { get; private set; } = string.Empty;
        public string DeviceName { get; private set; } = string.Empty;
        public long ConfigId { get; private set; }
        public DeviceType DeviceType { get; private set; }
        public string ProviderCode { get; private set; } = string.Empty;
        public string? BatchId { get; private set; }
        public DeviceBindState BindState { get; private set; }
        public DeviceBindType BindType { get; private set; }
        public string MchNo { get; private set; } = string.Empty;
        public string AppId { get; private set; } = string.Empty;
        public long StoreId { get; private set; }
        public long BindQrcId { get; private set; }
        public bool State { get; private set; }
        public string? Remark { get; private set; }
        public string SysType { get; private set; } = string.Empty;
        public string BelongInfoId { get; private set; } = string.Empty;

        private DeviceInfo()
        {
            DeviceInfoId = SnowflakeIdGenerator.GetSnowflakeId();
        }

        public static DeviceInfo Create(
            string deviceNo,
            string deviceName,
            long configId,
            DeviceType deviceType,
            string providerCode,
            string sysType,
            string belongInfoId,
            string? batchId = null,
            string? remark = null,
            bool state = true)
        {
            if (string.IsNullOrWhiteSpace(deviceNo))
                throw new ArgumentException("Device number cannot be empty", nameof(deviceNo));

            return new DeviceInfo
            {
                DeviceNo = deviceNo,
                DeviceName = deviceName,
                ConfigId = configId,
                DeviceType = deviceType,
                ProviderCode = providerCode,
                BatchId = batchId,
                SysType = sysType,
                BelongInfoId = belongInfoId,
                Remark = remark,
                State = state,
                BindState = DeviceBindState.Unbound
            };
        }

        public void BindToStore(string mchNo, string appId, long storeId)
        {
            if (BindState == DeviceBindState.Bound)
                throw new InvalidOperationException("Device already bound");

            MchNo = mchNo;
            AppId = appId;
            StoreId = storeId;
            BindType = DeviceBindType.Store;
            BindState = DeviceBindState.Bound;
        }

        public void BindToQrCode(string mchNo, string appId, long bindQrcId)
        {
            if (BindState == DeviceBindState.Bound)
                throw new InvalidOperationException("Device already bound");

            MchNo = mchNo;
            AppId = appId;
            BindQrcId = bindQrcId;
            BindType = DeviceBindType.QrCode;
            BindState = DeviceBindState.Bound;
        }

        public void Unbind()
        {
            if (BindState == DeviceBindState.Unbound) return;

            MchNo = string.Empty;
            AppId = string.Empty;
            StoreId = 0;
            BindQrcId = 0;
            BindState = DeviceBindState.Unbound;
        }

        public void Enable() => State = true;
        public void Disable() => State = false;
    }
}
