using webapi.Application.Users.Authenticate;
using webapi.Application.Users.Common;
using webapi.Application.Users.Delete;
using webapi.Application.Users.Exists;
using webapi.Application.Users.Get;
using webapi.Application.Users.Resister;
using webapi.Application.Users.Update;
using webapi.Domain.Models.Users;
using webapi.Domain.Service;
using System.Transactions;

namespace webapi.Application.Users
{
    public class UserApplicationService
    {
        private readonly IUserFactory _userFactory;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        public UserApplicationService(
            IUserFactory userFactory, 
            IUserRepository userRepository,
            IUserService userService
        )
        {
            _userFactory = userFactory;
            _userRepository = userRepository;
            _userService = userService;
        }

        public bool Exists(UserExistsCommand command)
        {
            return _userRepository.Exists(command.Id);
        }

        public bool Authenticate(UserAuthenticateCommand command)
        {
            User? user = _userRepository.Find(new UserId(command.Id));
            if (user == null)
            {
                return false;
            }
            else
            {
                return _userService.Verify(user, command.Password);
            }
        }

        public UserRegisterResult Register(UserRegisterCommand command)
        {
            using (var transaction = new TransactionScope())
            {
                var id = new UserId(command.Id);
                var name = new UserName(command.Name);
                var password = new UserPassword(command.Password);
                var user = _userFactory.Create(id, name, password);
                if (_userService.Exists(user))
                {
                    throw new CanNotRegisterUserException(user, "ユーザは既に存在しています。");
                }

                _userRepository.Save(user);

                transaction.Complete();

                return new UserRegisterResult(user.Id.Value);
            }
        }

        public UserGetResult Get (UserGetCommand command)
        {
            var id = new UserId(command.Id);
            var user = _userRepository.Find(id);
            if (user == null)
            {
                throw new UserNotFoundException(id, "ユーザが見つかりませんでした。");
            }
            var data = new UserData(user);
            return new UserGetResult(data);
        }

        public void Update(UserUpdateCommand command)
        {
            using (var transaction = new TransactionScope())
            {
                var id = new UserId(command.Id);
                var user = _userRepository.Find(id);
                if (user == null)
                {
                    return;
                }

                if (command.Name != null) user.ChangeName(new UserName(command.Name));
                if (command.Password != null) user.ChangePassword(new UserPassword(command.Password));
                if (command.Type != null) user.Type = (UserType)command.Type;
                if (command.State != null) user.State = (UserState)command.State;

                _userRepository.Save(user);

                transaction.Complete();
            }
        }

        public void Delete(UserDeleteCommand command)
        {
            using (var transaction = new TransactionScope())
            {
                var id = new UserId(command.Id);
                var user = _userRepository.Find(id);
                if (user == null)
                {
                    return;
                }

                _userRepository.Delete(user);

                transaction.Complete();
            }
        }
    }
}
