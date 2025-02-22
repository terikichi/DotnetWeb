using webapi.Domain.Models.Users;
using webapi.SQLInfrastructure.Provider;

namespace webapi.SQLInfrastructure.Persistence.Users
{
    public class SqlUserFactory : IUserFactory
    {
        private readonly DatabaseConnectionProvider provider;

        public SqlUserFactory(DatabaseConnectionProvider provider)
        {
            this.provider = provider;
        }

        public User Create(UserId id, UserName name, UserPassword password)
        {
            return new User(
                id,
                name,
                password
            );
        }
    }
}