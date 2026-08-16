using System;
using System.Collections.Generic;
using System.Text;

namespace Zentin.Payment.Domain.Enums
{

    /// <summary>
    /// Pay Order Status
    /// </summary>
    public enum PayOrderStatus
    {
        Created = 0,
        Paying = 1,
        Success = 2,
        Failed = 3,
        Revoked = 4,
        Refunded = 5,
        Closed = 6
    }
}
