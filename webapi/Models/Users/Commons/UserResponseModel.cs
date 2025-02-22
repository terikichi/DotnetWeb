using webapi.Application.Users.Common;

namespace webapi.Models.Users.Commons
{
    public class UserResponseModel
    {
        public UserResponseModel(UserData source) : this(source.Id, source.Name)
        {
        }

        public UserResponseModel(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; }
        public string Name { get; }
    }
}