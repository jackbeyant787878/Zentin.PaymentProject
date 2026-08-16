using System;
using System.Collections.Generic;
using System.Text;

namespace Zentin.Payment.Domain.Enums
{
    // <summary>
    /// Type of Agent /// 
    //</summary>
    public enum AgentType
    {
        Personal = 1, // Individual
        Enterprise = 2 // Enterprise
                       
    }
    /// <summary>
    /// Agent Status /// </summary>
    public enum AgentState
    {
        Disabled = 0,
        Normal = 1
    }
    /// <summary>
    /// Settlement Account Type /// </summary>
    public enum SettAccountType
    {
        AlipayCash,       // Personal Alipay
        WxCash,           // Personal WeChat
        BankPrivate,      // Personal Account
        BankPublic,       // Corporate Account
        BankCard          // Bank Card
                          
    }
    /// <summary>
    /// Withdrawal Fee Rules Type
    /// </summary>
    public enum CashoutFeeRuleType
    {
        SystemDefault = 1,
        Custom = 2
    }

  
    /// <summary>
    /// Account Billing Business Type /// </summary>
    public enum AccountBillBizType
    {
        OrderCommission = 1,  // Calculation of order commission
        RefundOffset = 2,     // Offset for refund
        CommissionWithdraw = 3,// Withdrawal of commission
        ManualAdjust = 4      // Manual account adjustment
    }



    /// <summary>
    /// Account Type
    /// </summary>
    public enum AccountType
    {
        Wallet = 1,  //wallet account
        InTransit = 2 //on-the-way account
    }

    /// <summary>
    /// Related Business Order Type
    /// </summary>
    public enum RelaBizOrderType
    {
        PayOrder = 1,
        RefundOrder = 2,
        WithdrawOrder = 3
    }

    /// <summary>
    /// Service Provider State
    /// </summary>
    public enum IsvState
    {
        Disabled = 0,
        Normal = 1
    }

    /// <summary>
    /// Configration Page Type
    /// </summary>
    public enum ConfigPageType
    {
        JsonRender = 1,
        Custom = 2
    }

    /// <summary>
    /// Fee Type
    /// </summary>
    public enum FeeType
    {
        Single, 
        Level   
    }

    /// <summary>
    /// level mode
    /// </summary>
    public enum LevelMode
    {
        Normal,
        UnionPay
    }

    /// <summary>
    /// Bank card type
    /// </summary>
    public enum BankCardType
    {
        Debit,  // 借记卡
        Credit  // 贷记卡
    }

    /// <summary>
    /// Merchant notification state
    /// </summary>
    public enum NotifyState
    {
        Notifying = 1,
        Success = 2,
        Failed = 3
    }

    /// <summary>
    /// Merchant notification order type
    /// </summary>
    public enum NotifyOrderType
    {
        Pay = 1,
        Refund = 2
    }
}
