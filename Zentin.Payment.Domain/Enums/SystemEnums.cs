namespace Zentin.Payment.Domain.Enums
{
    public enum SysUserState
    {
        Disabled = 0,
        Normal = 1
    }

    public enum SysUserType
    {
        SuperAdmin = 1,
        Operator = 2,
        MchExtender = 3,
        StoreManager = 11,
        StoreClerk = 12
    }

    public enum Sex
    {
        Unknown = 0,
        Male = 1,
        Female = 2
    }

    public enum IdentityType
    {
        Nickname = 1,
        Telphone = 2,
        Email = 3,
        Wechat = 10,
        QQ = 11,
        Alipay = 12,
        Weibo = 13
    }

    public enum ArticleType
    {
        Announcement = 1
    }
}