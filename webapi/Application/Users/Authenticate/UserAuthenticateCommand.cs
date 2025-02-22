namespace webapi.Application.Users.Authenticate
{
    public class UserAuthenticateCommand
    {
        public UserAuthenticateCommand(string id, string password)
        {
            Id = id;
            Password = password;
        }

        public string Id { get; }
        public string Password { get; }
    }
}
