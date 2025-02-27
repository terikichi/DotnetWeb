using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using webapi.Config.Dependancy;

var builder = WebApplication.CreateBuilder(args);
var factory = new DependencySetupFactory();
var setup = factory.CreateSetup(builder.Configuration);
setup.Run(builder.Services);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddCookie() // Cookie認証を有効にする
.AddJwtBearer(options =>
{
    var JwtKeySting = builder.Configuration["Jwt:Key"];

    if(JwtKeySting != null)
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKeySting)),
            ClockSkew = TimeSpan.Zero
        };
        options.Events = new JwtBearerEvents()
        {
            OnMessageReceived = context =>
            {
                Console.WriteLine(context.Request.Cookies);
                if (context.Request.Cookies.ContainsKey("X-Access-Token"))
                {
                    // "X-Access-Tokenのcookieが存在する場合はこの値を認証トークンとして扱う
                    context.Token = context.Request.Cookies["X-Access-Token"];
                }
                return Task.CompletedTask;
            }
        };
    }
    else
    {
        throw new NullReferenceException("JwtKeyが設定されていません。");
    }
});

var app = builder.Build();

// Configureメソッド内でクッキーを使用する設定
app.UseCookiePolicy(new CookiePolicyOptions
{
    HttpOnly = HttpOnlyPolicy.Always, // HttpOnly Cookieを使用
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
//    app.UseSwagger();
//    app.UseSwaggerUI();
}
else
{
    app.UseDefaultFiles();
    app.UseStaticFiles();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
