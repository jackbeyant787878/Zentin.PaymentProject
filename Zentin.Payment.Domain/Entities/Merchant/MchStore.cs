using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Common;
using Zentin.Payment.Domain.Abstractions;

namespace Zentin.Payment.Domain.Entities.Merchant
{
    public sealed class MchStore : BaseEntity
    {
        public long StoreId { get; private set; }
        public string StoreName { get; private set; } = string.Empty;
        public string MchNo { get; private set; } = string.Empty;
        public string? AgentNo { get; private set; }
        public string? IsvNo { get; private set; }
        public string ContactPhone { get; private set; } = string.Empty;
        public string StoreLogo { get; private set; } = string.Empty;
        public string StoreOuterImg { get; private set; } = string.Empty;
        public string StoreInnerImg { get; private set; } = string.Empty;
        public string? Remark { get; private set; }
        public string ProvinceCode { get; private set; } = string.Empty;
        public string CityCode { get; private set; } = string.Empty;
        public string DistrictCode { get; private set; } = string.Empty;
        public string Address { get; private set; } = string.Empty;
        public string Lng { get; private set; } = string.Empty;
        public string Lat { get; private set; } = string.Empty;
        public int DefaultFlag { get; private set; }
        public string? BindAppId { get; private set; }
        public long? CreatedUid { get; private set; }
        public string? CreatedBy { get; private set; }

        public MchInfo MchInfo { get; private set; } = null!;

        
        public MchStore()
        {
            StoreId = SnowflakeIdGenerator.GetSnowflakeId();
        }
    }
}
