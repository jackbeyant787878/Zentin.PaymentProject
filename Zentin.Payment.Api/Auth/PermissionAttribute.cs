namespace Zentin.Payment.Api.Auth
{
    /// <summary>
    /// 权限码元数据。本轮不做认证（网关接线时统一 enforcement），端点先带显式权限码。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class PermissionAttribute(string code) : Attribute
    {
        public string Code { get; } = code;
    }
}