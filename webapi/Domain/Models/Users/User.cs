using webapi.Common.Objects;

namespace webapi.Domain.Models.Users
{
    public class User
    {
        public User(UserId id, UserName name, UserPassword password, UserType type = UserType.Normal, UserState state = UserState.Active)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (password == null) throw new ArgumentNullException(nameof(password));

            Id = id;
            Name = name;
            Password = password;
            Type = type;
            State = state;
        }

        public UserId Id { get; private set; }
        public UserName Name { get; private set; }
        public UserPassword Password { get; private set; }
        public UserType Type { get; set; }
        public UserState State { get; set; }


        public void ChangeName(UserName name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            Name = name;
        }

        public void ChangePassword(UserPassword password)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));

            Password = password;
        }

        public override string ToString()
        {
            var sb = new ObjectValueStringBuilder(nameof(Id), Id)
                .Append(nameof(Name), Name);

            return sb.ToString();
        }
    }

    public enum UserType
    {
        Normal = 1,
        Premium = 2,
        SystemOperator = 9,
        SystemAdministrator = 0
    }

    public enum UserState
    {
        Active = 0,
        Suspend = 1,
        Banned = 9
    }
}
