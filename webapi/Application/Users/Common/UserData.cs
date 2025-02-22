using webapi.Domain.Models.Users;

namespace webapi.Application.Users.Common
{
    public class UserData
    {
        public UserData(User user) : this(user.Id.Value, user.Name.Value, user.Type, user.State)
        {
        }

        public UserData(string id, string name, UserType type, UserState state)
        {
            Id = id;
            Name = name;
            Type = type;
            State = state;
        }

        public string Id { get; }
        public string Name { get; }
        public UserType Type { get; }
        public UserState State { get; } 
    }
}
