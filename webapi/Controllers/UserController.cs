using webapi.Application.Users;
using webapi.Application.Users.Authenticate;
using webapi.Application.Users.Delete;
using webapi.Application.Users.Exists;
using webapi.Application.Users.Resister;
using webapi.Application.Users.Update;
using webapi.Domain.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserApplicationService _userApplicationService;

        public UserController(UserApplicationService userApplicationService)
        {
            _userApplicationService = userApplicationService;
        }

        [HttpGet("Exists")]
        public ActionResult<bool> Exists(string id)
        {
            var command = new UserExistsCommand(id);
            var result = _userApplicationService.Exists(command);
            return Ok(result);
        }

        [HttpPost("SignUp")]
        public IActionResult SignUp([FromBody] SignUpRequest signUpRequest)
        {
            var existsCommand = new UserExistsCommand(signUpRequest.Id);

            if (_userApplicationService.Exists(existsCommand))
            {
                return BadRequest(new {name = "id", message = "このIDは使用されています。" });
            }

            var command = new UserRegisterCommand(
                signUpRequest.Id, 
                signUpRequest.Name, 
                signUpRequest.Password
            );

            UserRegisterResult result = _userApplicationService.Register(command);

            return Ok(result);
        }

        [HttpPost("Edit")]
        [Authorize]
        public IActionResult Edit([FromBody] EditRequest request)
        {
            var command = new UserUpdateCommand(id:GetJwtId(), name:request.Name);
            _userApplicationService.Update(command);

            return Ok(true);
        }

        [HttpPost("ChangePassword")]
        [Authorize]
        public IActionResult ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var result = _userApplicationService.Authenticate(new UserAuthenticateCommand(GetJwtId(), request.CurrentPassword));

            if (!result)
            {
                return Unauthorized(new { name = "currentPassword", message = "現在のパスワードが間違っています。" });
            }

            if (request.NewPassword != request.ConfirmPassword)
            {
                return BadRequest(new { name = "confirmPassword", message = "新しいパスワードと一致させてください。" });
            }

            var command = new UserUpdateCommand(id:GetJwtId(), password:request.NewPassword);
            _userApplicationService.Update(command);

            return Ok(true);
        }

        [HttpPost("Delete")]
        [Authorize]
        public IActionResult Delete([FromBody] DeleteRequest request)
        {
            var result =_userApplicationService.Authenticate(new UserAuthenticateCommand(GetJwtId(), request.Password));

            if (!result)
            {
                return Unauthorized(new { name = "password", message = "パスワードが間違っています。" });
            }
            else
            {
                _userApplicationService.Delete(new UserDeleteCommand(GetJwtId()));

                // Http Only CookieからJWTトークンを削除
                Response.Cookies.Delete("X-Access-Token");

                return Ok(true);
            }
        }

        public string GetJwtId()
        {
            var JwtId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            
            if(JwtId != null)
            {
                return JwtId;
            }
            else
            {
                throw new NullReferenceException("IDが取得できませんでした");
            }
        }
    }

    public class SignUpRequest
    {
        public SignUpRequest(string id, string name, string password, string confirmPassword)
        {
            Id = id;
            Name = name;
            Password = password;
            ConfirmPassword = confirmPassword;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class DeleteRequest
    {
        public DeleteRequest(string password)
        {
            Password = password;
        }
        public string Password { get; set; }
    }

    public class EditRequest
    {
        public EditRequest(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public UserType UserType { get; set; }
        public UserState State { get; set; }
    }

    public class ChangePasswordRequest
    {
        public ChangePasswordRequest(string currentPassword, string newPassword, string confirmPassword)
        {
            CurrentPassword = currentPassword;
            NewPassword = newPassword;
            ConfirmPassword = confirmPassword;
        }

        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
