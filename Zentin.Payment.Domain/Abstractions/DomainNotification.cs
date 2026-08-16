using System;
using System.Collections.Generic;
using System.Text;
using Zentin.Payment.Domain.Abstractions;

namespace Zentin.Payment.Application.Contracts.Notifications
{
    /// <summary>
    /// domain notification event
    /// </summary>
    /// <remarks>
    ///  use for broadcasting domain validation information, error messages, status notifications, etc.
    /// </remarks>
    public class DomainNotification : IDomainEvent
    {
        /// <summary>
        ///  error code / business key
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// notification / error message content
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// notification level: Info / Warning / Error
        /// </summary>
        public NotificationLevel Level { get; }

        public DateTime OccurredOnUtc => DateTime.UtcNow;

        public DomainNotification(string key, string message, NotificationLevel level = NotificationLevel.Error)
        {
            Key = key;
            Message = message;
            Level = level;
        }
    }

    /// <summary>
    /// notification level
    /// </summary>
    public enum NotificationLevel : byte
    {
        Info = 0,
        Warning = 1,
        Error = 2
    }
}
