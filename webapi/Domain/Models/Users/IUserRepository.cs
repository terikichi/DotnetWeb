namespace webapi.Domain.Models.Users
{
    public interface IUserRepository
    {
        bool Exists(string id);
        User? Find(UserId id);
        User? Find(UserName name);
        List<User> Find(IEnumerable<UserId> ids);
        List<User> FindAll();
        void Save(User user);
        void Delete(User user);
    }
}
