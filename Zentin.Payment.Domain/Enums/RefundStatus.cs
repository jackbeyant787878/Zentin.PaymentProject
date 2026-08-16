using System;
using System.Collections.Generic;
using System.Text;

namespace Zentin.Payment.Domain.Enums
{
    /// <summary>
    /// Order refund status 
    /// </summary>
    public enum RefundStatus
    {
        NoRefund = 0,
        Partial = 1,
        Full = 2
    }

}
