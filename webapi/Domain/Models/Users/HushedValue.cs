using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace webapi.Domain.Models.Users
{
    public class HashedValue
    {
        public HashedValue(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public HashedValue Hash()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(Value));
                Value = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return this;
            }
        }
    }
}
