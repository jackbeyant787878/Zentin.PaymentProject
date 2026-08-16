namespace Zentin.Payment.Domain.Enums
{
    /// <summary>
    /// MerchantType
    /// </summary>
    public enum MchType
    {
        Normal = 1,    // normal merchant
        IsvSub = 2     // special merchant (service provider mode)
    }

    /// <summary>
    /// Merchant Type
    /// </summary>
    public enum MchState
    {
        Disabled = 0,  
        Normal = 1     
    }

    /// <summary>
    /// Merchant Apply Status
    /// </summary>
    public enum MchApplyState
    {
        Draft = 0,             // Draft
        Auditing = 1,          // Auditing
        Success = 2,           //Success
        Rejected = 3,          // Rejected
        PendingVerify = 4,     // Pending Verify
        PendingSign = 5,       // Pending Sign
        PendingPreAudit = 7,   // Pending Pre Audit
        PreAuditRejected = 8   // Pre Audit Rejected
    }

    /// <summary>
  
    /// merchant subject type
    /// </summary>
    public enum MerchantSubjectType
    {
        Personal = 1,      // personal business 
        Individual = 2,    // individual business
        Enterprise = 3     // enterprise business
    }

    /// <summary>
    /// Merchant Application Status
    /// </summary>
    public enum MchAppState
    {
        Disabled = 0,
        Normal = 1
    }

    /// <summary>
    /// Membership State
    /// </summary>
    public enum MbrState
    {
        Disabled = 0,
        Normal = 1
    }

    /// <summary>
    /// Membership billing business type
    /// </summary>
    public enum MbrBillBizType
    {
        PayRecharge = 1,    // Make payment for recharge
        CashRecharge = 2,    // Cash recharge
        Consume = 3,        // Membership consumption
        Refund = 4,         // Consumer refund
        ManualAdjust = 5    // Manual account adjustment
    }

    /// <summary>
    /// Membership recharge order status
    /// </summary>
    public enum MbrRechargeState
    {
        Init = 0,
        Paying = 1,
        Success = 2,
        Failed = 3
    }
}
