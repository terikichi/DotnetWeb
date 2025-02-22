namespace webapi.Application.Users.Exists
{
    public class UserExistsCommand
    {
        public UserExistsCommand(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
