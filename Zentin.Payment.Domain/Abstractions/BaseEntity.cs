namespace Zentin.Payment.Domain.Abstractions;

public interface IAuditable
{
    DateTime CreatedOnUtc { get; set; }
    DateTime UpdatedOnUtc { get; set; }
}


public abstract class BaseEntity : IAuditable
{
    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedOnUtc { get; set; } = DateTime.UtcNow;
}
