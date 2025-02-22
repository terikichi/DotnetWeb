namespace webapi.Application.Users.Resister
{
    public class UserRegisterCommand
    {
        public UserRegisterCommand(string id ,string name, string password)
        {
            Id = id;
            Name = name;
            Password = password;
        }

        public string Id { get; }
        public string Name { get; }
        public string Password { get; }
    }
}
