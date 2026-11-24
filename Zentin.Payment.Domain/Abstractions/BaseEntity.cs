namespace Zentin.Payment.Domain.Abstractions;

public interface IAuditable
{
    DateTime CreatedOnUtc { get; }
    DateTime UpdatedOnUtc { get; }
}

public abstract class BaseEntity : IAuditable
{
    // 审计时间只允许基础设施拦截器经 EF 属性写入，领域代码不可篡改
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedOnUtc { get; private set; } = DateTime.UtcNow;
}