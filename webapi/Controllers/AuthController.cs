using webapi.Application.Users;
using webapi.Application.Users.Authenticate;
using webapi.Application.Users.Exists;
using webapi.Application.Users.Get;
using webapi.Domain.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly UserApplicationService _userApplicationService;

    public AuthController(IConfiguration configuration, UserApplicationService userApplicationService)
    {
        _configuration = configuration;
        _userApplicationService = userApplicationService;
    }

    [HttpPost("Login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {

        if (!IsValidUser(request.Id))
        {
            return Unauthorized(new { name = "id" , message = "IDが存在しません。" });
        }

        if (IsValidPassword(request.Id, request.Password))
        {
            // トークンを生成
            var token = GenerateJwtToken(request.Id);

            // クッキーにトークンを設定（HttpOnlyを有効にする）
            Response.Cookies.Append("X-Access-Token", token, new CookieOptions
            {
                HttpOnly = true, // Http Only Cookieにする
                Secure = true, // HTTPS接続時のみ送信
                SameSite = SameSiteMode.Strict, // SameSite属性を設定（CSRF対策）
            });

            return Ok();
        }
        else
        {
            return Unauthorized(new { name = "password", message = "パスワードが間違っています。" });
        }
    }

    [HttpGet("RequestUser")]
    [Authorize]
    public IActionResult RequestUser() {
        var Id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if(Id != null)
        {
            var user = _userApplicationService.Get(new UserGetCommand(Id)).Data;
            var response = new UserResponse(user.Id, user.Name, user.Type, user.State);
            return Ok(response);
        }
        else
        {
            return Unauthorized(new {message = "IDが取得できませんでした。"});
        }
    }

    private bool IsValidUser(string id)
    {
        return _userApplicationService.Exists(new UserExistsCommand(id));
    }

    private bool IsValidPassword(string id, string password)
    {
        return _userApplicationService.Authenticate(new UserAuthenticateCommand(id, password));
    }

    private string GenerateJwtToken(string Id)
    {
        var JwtKeyString = _configuration["Jwt:Key"];
        if(JwtKeyString != null)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKeyString));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, Id)
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        else
        {
            throw new NullReferenceException("JwtKeyが取得できませんでした。");
        }
    }

    [HttpPost("Logout")]
    public IActionResult Logout()
    {
        // Http Only CookieからJWTトークンを削除
        Response.Cookies.Delete("X-Access-Token");

        // ログアウト成功の応答を返す
        return Ok();
    }

    public class LoginRequest
    {
        public LoginRequest(string id, string password)
        {
            Id = id;
            Password = password;
        }

        public string Id { get; set; }
        public string Password { get; set; }
    }

    public class UserResponse
    {
        public UserResponse(string id, string name, UserType type, UserState state)
        {
            Id = id;
            Name = name;
            Type = type;
            State = state;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public UserType Type { get; set; }
        public UserState State { get; set; }
    }
}

