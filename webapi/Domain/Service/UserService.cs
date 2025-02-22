using webapi.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webapi.Domain.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public bool Exists(User user)
        {
            var duplicatedUser = userRepository.Find(user.Name);

            return duplicatedUser != null;
        }

        public bool Verify(User user, string plainPassword)
        {
            if (user.State != UserState.Active) return false;

            // パスワードの照合
            UserPassword inputHash = new UserPassword(plainPassword);
            return user.Password.Equals(inputHash);
        }
    }
}
