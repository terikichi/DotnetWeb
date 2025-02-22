namespace webapi.Application.Users.Resister
{
    public class UserRegisterResult
    {
        public UserRegisterResult(string createdUserId)
        {
            CreatedUserId = createdUserId;
        }

        public string CreatedUserId { get; }
    }
}