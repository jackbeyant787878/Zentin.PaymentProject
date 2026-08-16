using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Common;
using Zentin.Payment.Domain.Abstractions;

namespace Zentin.Payment.Domain.Entities.Division
{
    /// <summary>
    /// Merchant division receiver group
    /// </summary>
    public sealed class MchDivisionReceiverGroup : BaseEntity
    {
        public long ReceiverGroupId { get; private set; }
        public string ReceiverGroupName { get; private set; } = string.Empty;
        public string MchNo { get; private set; } = string.Empty;
        public bool AutoDivisionFlag { get; private set; }
        public long? CreatedUid { get; private set; }
        public string? CreatedBy { get; private set; }

        private MchDivisionReceiverGroup()
        {
            ReceiverGroupId = SnowflakeIdGenerator.GetSnowflakeId();
        }

        /// <summary>
        /// Create a receiver group
        /// </summary>
        public static MchDivisionReceiverGroup Create(
            string groupName,
            string mchNo,
            bool autoDivisionFlag = false,
            long? createdUid = null,
            string? createdBy = null)
        {
            if (string.IsNullOrWhiteSpace(groupName))
                throw new ArgumentException("Group name cannot be empty", nameof(groupName));
            if (string.IsNullOrWhiteSpace(mchNo))
                throw new ArgumentException("Merchant number cannot be empty", nameof(mchNo));

            return new MchDivisionReceiverGroup
            {
                ReceiverGroupName = groupName,
                MchNo = mchNo,
                AutoDivisionFlag = autoDivisionFlag,
                CreatedUid = createdUid,
                CreatedBy = createdBy
            };
        }

        /// <summary>
        /// Enable auto division for this group
        /// </summary>
        public void EnableAutoDivision() => AutoDivisionFlag = true;

        /// <summary>
        /// Disable auto division for this group
        /// </summary>
        public void DisableAutoDivision() => AutoDivisionFlag = false;

        /// <summary>
        /// Rename group
        /// </summary>
        public void Rename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Group name cannot be empty", nameof(newName));
            ReceiverGroupName = newName;
        }
    }
}
