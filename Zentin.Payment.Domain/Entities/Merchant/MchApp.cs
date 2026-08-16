using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Domain.Entities.Merchant
{
    public sealed class MchApp : BaseEntity
    {
        public string AppId { get; private set; } = string.Empty;
        public string AppName { get; private set; } = string.Empty;
        public string MchNo { get; private set; } = string.Empty;
        public MchAppState State { get; private set; }
        public int DefaultFlag { get; private set; }
        public List<string> AppSignType { get; private set; } = [];
        public string AppSecret { get; private set; } = string.Empty;
        public string? AppRsa2PublicKey { get; private set; }
        public string? Remark { get; private set; }
        public long? CreatedUid { get; private set; }
        public string? CreatedBy { get; private set; }

        
        // navigational property for the associated merchant
        public MchInfo MchInfo { get; private set; } = null!;

        private MchApp() { }

        
        /// <summary>
        /// Creates a new instance of MchApp with the specified parameters.
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appName"></param>
        /// <param name="appSecret"></param>
        /// <param name="defaultFlag"></param>
        /// <param name="remark"></param>
        /// <param name="createdUid"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        internal static MchApp Create(
            string appId,
            string appName,
            string appSecret,
            int defaultFlag = 0,
            string? remark = null,
            long? createdUid = null,
            string? createdBy = null)
        {
            return new MchApp
            {
                AppId = appId,
                AppName = appName,
                AppSecret = appSecret,
                DefaultFlag = defaultFlag,
                AppSignType = ["MD5", "RSA2"],
                State = MchAppState.Normal,
                Remark = remark,
                CreatedUid = createdUid,
                CreatedBy = createdBy
            };
        }
    }
}
