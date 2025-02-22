using webapi.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webapi.Application.Users.Update
{
    public class UserUpdateCommand
    {

        public UserUpdateCommand(string id, string? name = null, string? password = null, UserType? type = null, UserState? state = null)
        {
            Id = id;
            Name = name;
            Password = password;
            Type = type;
            State = state;
        }

        public string Id { get; private set; }
        public string? Name { get; private set; }
        public string? Password { get; private set; }
        public UserType? Type { get; private set; }
        public UserState? State { get; private set; }
    }
}