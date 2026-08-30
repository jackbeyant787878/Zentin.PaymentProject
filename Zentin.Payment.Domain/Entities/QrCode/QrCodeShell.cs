using Zentin.Payment.Common;
using Zentin.Payment.Domain.Abstractions;

namespace Zentin.Payment.Domain.Entities.QrCode
{
    /// <summary>
    /// QR code shell template
    /// </summary>
    public sealed class QrCodeShell : BaseEntity
    {
        public long QrCodeShellId { get; private set; }
        public string StyleCode { get; private set; } = string.Empty;
        public string ShellAlias { get; private set; } = string.Empty;
        public string ConfigInfo { get; private set; } = string.Empty;
        public string SysType { get; private set; } = string.Empty;
        public string BelongInfoId { get; private set; } = string.Empty;

        private QrCodeShell()
        {
            QrCodeShellId = SnowflakeIdGenerator.GetSnowflakeId();
        }

        public static QrCodeShell Create(string styleCode, string shellAlias, string configInfo, string sysType, string belongInfoId)
        {
            if (string.IsNullOrWhiteSpace(styleCode))
                throw new ArgumentException("Style code cannot be empty", nameof(styleCode));
            if (string.IsNullOrWhiteSpace(configInfo))
                throw new ArgumentException("Config info cannot be empty", nameof(configInfo));

            return new QrCodeShell
            {
                StyleCode = styleCode,
                ShellAlias = shellAlias,
                ConfigInfo = configInfo,
                SysType = sysType,
                BelongInfoId = belongInfoId
            };
        }

        public void UpdateConfig(string configInfo, string shellAlias)
        {
            ConfigInfo = configInfo;
            ShellAlias = shellAlias;
        }
    }
}
