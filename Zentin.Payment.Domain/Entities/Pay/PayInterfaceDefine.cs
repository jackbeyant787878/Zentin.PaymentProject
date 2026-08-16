using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Enums;

namespace Zentin.Payment.Domain.Entities.Pay
{
    public sealed class PayInterfaceDefine : BaseEntity
    {
        public string IfCode { get; private set; } = string.Empty;
        public string IfName { get; private set; } = string.Empty;
        public bool IsMchMode { get; private set; }
        public bool IsIsvMode { get; private set; }
        public ConfigPageType ConfigPageType { get; private set; }
        public bool IsSupportApplyment { get; private set; }
        public bool IsOpenApplyment { get; private set; }
        public bool IsSupportCheckBill { get; private set; }
        public bool IsOpenCheckBill { get; private set; }
        public bool IsSupportCashout { get; private set; }
        public bool IsOpenCashout { get; private set; }
        public string? IsvParams { get; private set; }
        public string? IsvsubMchParams { get; private set; }
        public string? NormalMchParams { get; private set; }
        public List<string> WayCodes { get; private set; } = [];
        public string? Icon { get; private set; }
        public string? BgColor { get; private set; }
        public bool State { get; private set; }
        public string? Remark { get; private set; }

        private PayInterfaceDefine() { }

      
        public static PayInterfaceDefine Create(
            string ifCode,
            string ifName,
            ConfigPageType configPageType,
            List<string> wayCodes,
            bool isMchMode = true,
            bool isIsvMode = true,
            string? icon = null,
            string? bgColor = null,
            string? remark = null,
            bool state = true)
        {
            if (string.IsNullOrWhiteSpace(ifCode))
                throw new ArgumentException("Payment Interface Code cannot be null or empty", nameof(ifCode));
            if (string.IsNullOrWhiteSpace(ifName))
                throw new ArgumentException("Payment Interface Name cannot be null or empty", nameof(ifName));
            if (wayCodes == null || wayCodes.Count == 0)
                throw new ArgumentException("At least one supported payment method must be configured", nameof(wayCodes));

            return new PayInterfaceDefine
            {
                IfCode = ifCode,
                IfName = ifName,
                ConfigPageType = configPageType,
                WayCodes = wayCodes,
                IsMchMode = isMchMode,
                IsIsvMode = isIsvMode,
                Icon = icon,
                BgColor = bgColor,
                Remark = remark,
                State = state,

              
                IsSupportApplyment = false,
                IsOpenApplyment = false,
                IsSupportCheckBill = false,
                IsOpenCheckBill = false,
                IsSupportCashout = false,
                IsOpenCashout = false
            };
        }

        // ========== business methods ==========
        /// <summary>
        /// set parameter templates
        /// </summary>
        public void SetParams(
            string? isvParams = null,
            string? isvsubMchParams = null,
            string? normalMchParams = null)
        {
            IsvParams = isvParams;
            IsvsubMchParams = isvsubMchParams;
            NormalMchParams = normalMchParams;
        }

        /// <summary>
        ///set applyment ability
        /// </summary>
        public void SetApplymentAbility(bool isSupport, bool isOpen)
        {
            IsSupportApplyment = isSupport;
            IsOpenApplyment = isOpen;
        }

        /// <summary>
        /// set check bill ability
        /// </summary>
        public void SetCheckBillAbility(bool isSupport, bool isOpen)
        {
            IsSupportCheckBill = isSupport;
            IsOpenCheckBill = isOpen;
        }

        /// <summary>
        /// set cashout ability
        /// </summary>
        public void SetCashoutAbility(bool isSupport, bool isOpen)
        {
            IsSupportCashout = isSupport;
            IsOpenCashout = isOpen;
        }

        /// <summary>
        /// update supported payment methods
        /// </summary>
        public void UpdateWayCodes(List<string> wayCodes)
        {
            if (wayCodes == null || wayCodes.Count == 0)
                throw new ArgumentException("At least one supported payment method must be configured", nameof(wayCodes));

            WayCodes = wayCodes;
        }

        public void Enable() => State = true;
        public void Disable() => State = false;

    }
}
