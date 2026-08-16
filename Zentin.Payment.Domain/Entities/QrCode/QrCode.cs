using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using static Zentin.Payment.Domain.Enums.Enums;

namespace Zentin.Payment.Domain.Entities.QrCode
{
    /// <summary>
    /// QR code entity
    /// </summary>
    public sealed class QrCode : BaseEntity
    {
        public string QrCodeId { get; private set; } = string.Empty;
        public int? QrcShellId { get; private set; }
        public string? BatchId { get; private set; }
        public bool FixedFlag { get; private set; }
        public int FixedPayAmount { get; private set; }
        public string EntryPage { get; private set; } = string.Empty;
        public string AlipayWayCode { get; private set; } = string.Empty;
        public string? QrcAlias { get; private set; }
        public QrCodeBindState BindState { get; private set; }
        public string? AgentNo { get; private set; }
        public string? MchNo { get; private set; }
        public string? AppId { get; private set; }
        public long? StoreId { get; private set; }
        public string? UrlPlaceholder { get; private set; }
        public bool State { get; private set; }
        public string SysType { get; private set; } = string.Empty;
        public string BelongInfoId { get; private set; } = string.Empty;

        private QrCode() { }

        public static QrCode Create(
            string qrcId,
            string entryPage,
            string alipayWayCode,
            string sysType,
            string belongInfoId,
            int? qrcShellId = null,
            string? batchId = null,
            bool fixedFlag = false,
            int fixedPayAmount = 0,
            string? qrcAlias = null)
        {
            if (string.IsNullOrWhiteSpace(qrcId))
                throw new ArgumentException("QR code ID cannot be empty", nameof(qrcId));

            return new QrCode
            {
                QrCodeId = qrcId,
                QrcShellId = qrcShellId,
                BatchId = batchId,
                FixedFlag = fixedFlag,
                FixedPayAmount = fixedPayAmount,
                EntryPage = entryPage,
                AlipayWayCode = alipayWayCode,
                QrcAlias = qrcAlias,
                BindState = QrCodeBindState.Unbound,
                State = true,
                SysType = sysType,
                BelongInfoId = belongInfoId
            };
        }

        public void BindToMerchant(string mchNo, string appId, long storeId, string? agentNo = null)
        {
            if (BindState == QrCodeBindState.Bound)
                throw new InvalidOperationException("QR code already bound");

            MchNo = mchNo;
            AppId = appId;
            StoreId = storeId;
            AgentNo = agentNo;
            BindState = QrCodeBindState.Bound;
        }

        public void Unbind()
        {
            if (BindState == QrCodeBindState.Unbound) return;

            MchNo = null;
            AppId = null;
            StoreId = null;
            AgentNo = null;
            BindState = QrCodeBindState.Unbound;
        }

        public void Enable() => State = true;
        public void Disable() => State = false;
    }
}
