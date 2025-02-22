using webapi.Common.Objects;
using webapi.Domain.Models.Users;
using Microsoft.Data.SqlClient;
using webapi.SQLInfrastructure.Provider;

namespace webapi.SQLInfrastructure.Persistence.Users
{
    public class SqlUserRepository : IUserRepository
    {
        private readonly DatabaseConnectionProvider provider;

        public SqlUserRepository(DatabaseConnectionProvider provider)
        {
            this.provider = provider;
        }

        public bool Exists(string id)
        {
            using (var command = provider.Connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(id) AS 'count' FROM users WHERE id = @id";
                command.Parameters.Add(new SqlParameter("@id", id));

                int count = 0;

                try
                {
                    count = (int)command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                return count > 0;
            }
        }

        public User? Find(UserId id)
        {
            using (var command = provider.Connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM users WHERE id = @id";
                command.Parameters.Add(new SqlParameter("@id", id.Value));

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var user = Read(reader);

                        return user;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public User? Find(UserName name)
        {
            using (var command = provider.Connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM users WHERE name = @name";
                command.Parameters.Add(new SqlParameter("@name", name.Value));

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var user = Read(reader);

                        return user;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public List<User> Find(IEnumerable<UserId> Ids)
        {
            var ids = Ids.ToList();
            if (!ids.Any())
            {
                throw new ArgumentException("empty.", nameof(Ids));
            }

            using (var command = provider.Connection.CreateCommand())
            {
                var parameterNames = ids.Select((_, i) => "@id" + i).ToList();

                var inParameters = string.Join(",", parameterNames);

                command.CommandText = $"SELECT * FROM users WHERE id IN ({inParameters})";


                foreach (var tpl in ids.Zip(parameterNames, (id, parameter) => new { id, parameter }))
                {
                    command.Parameters.Add(new SqlParameter(tpl.parameter, tpl.id));
                }

                using (var reader = command.ExecuteReader())
                {
                    var results = new List<User>();
                    while (reader.Read())
                    {
                        var user = Read(reader);
                        results.Add(user);
                    }

                    return results;
                }
            }
        }

        public List<User> FindAll()
        {
            using (var command = provider.Connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM users";

                using (var reader = command.ExecuteReader())
                {
                    var results = new List<User>();
                    while (reader.Read())
                    {
                        var user = Read(reader);
                        results.Add(user);
                    }

                    return results;
                }
            }
        }

        public void Save(User user)
        {
            using (var command = provider.Connection.CreateCommand())
            {
                command.CommandText = @"
MERGE INTO users AS target
USING (
    SELECT
        @id AS id, 
        @name AS name, 
        @password AS password, 
        @type AS type, 
        @state AS state
) AS data
ON target.id = data.id 
WHEN MATCHED THEN
    UPDATE SET 
        target.name = data.name, 
        target.password = data.password, 
        target.type = data.type, 
        target.state = data.state,
        target.updated_at = GETDATE()
WHEN NOT MATCHED THEN
    INSERT (id, name, password, type, state, created_at, updated_at)
    VALUES (data.id, data.name, data.password, data.type, data.state, GETDATE(), GETDATE());
";
                command.Parameters.Add(new SqlParameter("@id", user.Id.Value));
                command.Parameters.Add(new SqlParameter("@name", user.Name.Value));
                command.Parameters.Add(new SqlParameter("@password", user.Password.HashedValue.Value));
                command.Parameters.Add(new SqlParameter("@type", (byte)user.Type));
                command.Parameters.Add(new SqlParameter("@state", (byte)user.State));

                command.ExecuteNonQuery();
            }
        }

        public void Delete(User user)
        {
            using (var command = provider.Connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM users WHERE id = @id";
                command.Parameters.Add(new SqlParameter("@id", user.Id.Value));
                command.ExecuteNonQuery();
            }
        }

        public User Read(SqlDataReader reader)
        {
            var id = (string)reader["id"];
            var name = (string)reader["name"];
            var password = (string)reader["password"];
            var type = (byte)reader["type"];
            var state = (byte)reader["state"];

            return new User(
                new UserId(id),
                new UserName(name),
                new UserPassword(new HashedValue(password)),
                (UserType)type,
                (UserState)state
            );
        }
    }
}