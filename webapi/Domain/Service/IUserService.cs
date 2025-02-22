using webapi.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webapi.Domain.Service
{
    public interface IUserService
    {
        public bool Exists(User user);

        public bool Verify(User user, string plainPassword);
    }
}
