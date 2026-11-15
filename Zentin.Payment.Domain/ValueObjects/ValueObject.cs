namespace Zentin.Payment.Domain.ValueObjects
{
    /// <summary>值对象基类：按分量判等，无标识。</summary>
    public abstract class ValueObject<T> where T : ValueObject<T>
    {
        public override bool Equals(object? obj)
        {
            return obj is T other && EqualsCore(other);
        }

        public override int GetHashCode() => GetHashCodeCore();

        protected abstract bool EqualsCore(T other);

        protected abstract int GetHashCodeCore();

        public static bool operator ==(ValueObject<T>? left, ValueObject<T>? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(ValueObject<T>? left, ValueObject<T>? right) => !(left == right);

        public virtual T Clone() => (T)MemberwiseClone();
    }
}