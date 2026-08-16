using System;
using System.Collections.Generic;
using System.Text;

namespace Zentin.Payment.Domain.Enums
{
    public class Enums
    {

        /// <summary>
        /// Merchant notify record state
        /// </summary>
        public enum NotifyState
        {
            Notifying = 1,
            Success = 2,
            Failed = 3
        }

        /// <summary>
        /// Notify order type
        /// </summary>
        public enum NotifyOrderType
        {
            Pay = 1,
            Refund = 2
        }

        /// <summary>
        /// Refund order status
        /// </summary>
        public enum RefundOrderStatus
        {
            Created = 0,
            Processing = 1,
            Success = 2,
            Failed = 3,
            Closed = 4
        }

        /// <summary>
        /// Transfer order status
        /// </summary>
        public enum TransferOrderStatus
        {
            Created = 0,
            Processing = 1,
            Success = 2,
            Failed = 3,
            Closed = 4
        }

        /// <summary>
        /// Division receiver account state
        /// </summary>
        public enum DivisionReceiverState
        {
            Suspended = 0,
            Normal = 1
        }

        /// <summary>
        /// Check bill parse status
        /// </summary>
        public enum CheckBillParseStatus
        {
            Failed = 0,
            Success = 1
        }

        /// <summary>
        /// Check bill handle state
        /// </summary>
        public enum CheckBillHandleState
        {
            Unhandled = 0,
            Handled = 1
        }

        /// <summary>
        /// Difference bill handle state
        /// </summary>
        public enum DiffBillHandleState
        {
            Unhandled = 0,
            Suspended = 1,
            Handled = 2,
            Ignored = 3
        }

        /// <summary>
        /// Device type
        /// </summary>
        public enum DeviceType
        {
            CloudSpeaker = 1,
            CloudPrinter = 2
        }

        /// <summary>
        /// Device bind state
        /// </summary>
        public enum DeviceBindState
        {
            Unbound = 0,
            Bound = 1
        }

        /// <summary>
        /// Device bind type
        /// </summary>
        public enum DeviceBindType
        {
            Store = 0,
            QrCode = 1
        }

        /// <summary>
        /// QR code bind state
        /// </summary>
        public enum QrCodeBindState
        {
            Unbound = 0,
            Bound = 1
        }

        /// <summary>
        /// Order snapshot type
        /// </summary>
        public enum OrderSnapshotType
        {
            Pay = 1,
            Refund = 2
        }
    }
}
