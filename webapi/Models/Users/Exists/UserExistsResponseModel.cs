using webapi.Models.Users.Commons;

namespace webapi.Models.Users.Exists
{
    public class UserExistsResponseModel
    {
        public UserExistsResponseModel(UserResponseModel user)
        {
            User = user;
        }

        public UserResponseModel User { get; set; }
    }
}