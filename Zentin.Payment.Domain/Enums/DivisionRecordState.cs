using System;
using System.Collections.Generic;
using System.Text;

namespace Zentin.Payment.Domain.Enums
{
    public enum DivisionRecordState
    {
        Pending = 0,
        Success = 1,
        Failed = 2,
        Accepted = 3
    }

    public enum DivisionReceiverAccType
    {
        Personal = 0,
        Merchant = 1
    }
}
