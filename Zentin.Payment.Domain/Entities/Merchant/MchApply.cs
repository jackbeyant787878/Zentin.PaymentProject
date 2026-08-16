using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;
using Zentin.Payment.Domain.Enums;
using Zentin.Payment.Domain.Events;

namespace Zentin.Payment.Domain.Entities.Merchant
{
    public sealed class MchApply : AggregateRoot
    {
        public string ApplyId { get; private set; } = string.Empty;
        public string MchNo { get; private set; } = string.Empty;
        public string AgentNo { get; private set; } = string.Empty;
        public string? TopAgentNo { get; private set; }
        public string? IsvNo { get; private set; }
        public string IfCode { get; private set; } = string.Empty;
        public string AutoConfigMchAppId { get; private set; } = string.Empty;
        public string ApplyPageType { get; private set; } = string.Empty;
        public string ApplyDetailInfo { get; private set; } = string.Empty;
        public string ApplyErrorInfo { get; private set; } = string.Empty;
        public string ChannelApplyNo { get; private set; } = string.Empty;
        public string SuccResParameter { get; private set; } = string.Empty;
        public string ChannelVar1 { get; private set; } = string.Empty;
        public string ChannelVar2 { get; private set; } = string.Empty;
        public MchApplyState State { get; private set; }
        public bool IsTempData { get; private set; }
        public string MchFullName { get; private set; } = string.Empty;
        public string MchShortName { get; private set; } = string.Empty;
        public MerchantSubjectType MerchantType { get; private set; }
        public string ContactName { get; private set; } = string.Empty;
        public string ContactPhone { get; private set; } = string.Empty;
        public string? ContactEmail { get; private set; }
        public string ProvinceCode { get; private set; } = string.Empty;
        public string CityCode { get; private set; } = string.Empty;
        public string DistrictCode { get; private set; } = string.Empty;
        public string Address { get; private set; } = string.Empty;
        public long? EpUserId { get; private set; }
        public DateTime LastApplyAt { get; private set; }
        public long? CreatedUid { get; private set; }
        public string? CreatedBy { get; private set; }

        private MchApply() { }

       
        /// <summary>
        /// Creates a new instance of MchApply with the provided parameters.
        /// </summary>
        /// <returns></returns>
        public static MchApply Create(
            string applyId,
            string mchNo,
            string agentNo,
            string ifCode,
            string autoConfigMchAppId,
            string applyPageType,
            string mchFullName,
            string mchShortName,
            MerchantSubjectType merchantType,
            string contactName,
            string contactPhone,
            string provinceCode,
            string cityCode,
            string districtCode,
            string address,
            bool isTempData = false,
            long? createdUid = null,
            string? createdBy = null)
        {
            var apply = new MchApply
            {
                ApplyId = applyId,
                MchNo = mchNo,
                AgentNo = agentNo,
                IfCode = ifCode,
                AutoConfigMchAppId = autoConfigMchAppId,
                ApplyPageType = applyPageType,
                MchFullName = mchFullName,
                MchShortName = mchShortName,
                MerchantType = merchantType,
                ContactName = contactName,
                ContactPhone = contactPhone,
                ProvinceCode = provinceCode,
                CityCode = cityCode,
                DistrictCode = districtCode,
                Address = address,
                IsTempData = isTempData,
                CreatedUid = createdUid,
                CreatedBy = createdBy,
                State = MchApplyState.Draft,
                LastApplyAt = DateTime.UtcNow
            };

            return apply;
        }

        /// <summary>
        /// business behavior: submit merchant application
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void Submit()
        {
            if (State != MchApplyState.Draft && State != MchApplyState.Rejected)
                throw new InvalidOperationException("only draft or rejected states can be submitted for review");

            State = MchApplyState.Auditing;
            LastApplyAt = DateTime.UtcNow;
            AddDomainEvent(new MchApplySubmittedDomainEvent(ApplyId, MchNo, IfCode, DateTime.UtcNow));
        }

        
        /// <summary>
        /// Marks the merchant application as successful and raises a domain event.
        /// </summary>
        /// <param name="channelApplyNo"></param>
        /// <param name="succResParameter"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void MarkSuccess(string channelApplyNo, string succResParameter)
        {
            if (State != MchApplyState.Auditing)
                throw new InvalidOperationException("only auditing state can be marked as successful");

            State = MchApplyState.Success;
            ChannelApplyNo = channelApplyNo;
            SuccResParameter = succResParameter;
            AddDomainEvent(new MchApplySuccessDomainEvent(ApplyId, MchNo, channelApplyNo, DateTime.UtcNow));
        }

       
        /// <summary>
        /// Marks the merchant application as rejected and sets the error information.
        /// </summary>
        /// <param name="errorInfo"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void Reject(string errorInfo)
        {
            if (State != MchApplyState.Auditing)
                throw new InvalidOperationException("only auditing state can be rejected");

            State = MchApplyState.Rejected;
            ApplyErrorInfo = errorInfo;
        }
    }
}
