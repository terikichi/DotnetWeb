namespace webapi.Domain.Models.Users
{
    public class UserPassword : IEquatable<UserPassword>
    {
        public UserPassword(string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (value.Length < 8) throw new ArgumentException("パスワードは8文字以上です。", nameof(value));
            if (value.Length > 16) throw new ArgumentException("パスワードは16文字以下です。", nameof(value));
            Value = value;
            HashedValue =  new HashedValue(value).Hash();
        }

        public UserPassword(HashedValue hashedValue)
        {
            if (hashedValue == null) throw new ArgumentNullException(nameof(hashedValue));
            HashedValue = hashedValue;
        }

        private string? Value { get; }
        public HashedValue HashedValue { get; }

        public bool Equals(UserPassword? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(HashedValue.Value, other.HashedValue.Value);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((UserPassword)obj);
        }

        public override string? ToString()
        {
            return Value;
        }

        public override int GetHashCode()
        {
            return HashedValue != null ? HashedValue.Value.GetHashCode() : 0;
        }
    }
}