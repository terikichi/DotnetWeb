using webapi.Application.Users.Common;

namespace webapi.Application.Users.Get
{
    public class UserGetResult
    {
        public UserGetResult(UserData data)
        {
            Data = data;
        }

        public UserData Data { get; set; }
    }
}