namespace webapi.Domain.Models.Users
{
    public class UserId : IEquatable<UserId>
    {
        public UserId(string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (value.Length < 3) throw new ArgumentException("ユーザIDは3文字以上です。", nameof(value));
            if (value.Length > 20) throw new ArgumentException("ユーザIDは20文字以下です。", nameof(value));
            Value = value;
        }

        public string Value { get; }

        public bool Equals(UserId? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Value, other.Value);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((UserId)obj);
        }

        public override int GetHashCode()
        {
            return Value != null ? Value.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
