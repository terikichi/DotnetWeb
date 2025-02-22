namespace webapi.Domain.Models.Users
{
    public interface IUserFactory
    {
        User Create(UserId id, UserName name, UserPassword password);
    }
}
