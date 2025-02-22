using webapi.Application.Users;
using webapi.Domain.Models.Users;
using webapi.Domain.Service;
using webapi.SQLInfrastructure.Persistence.Users;
using webapi.SQLInfrastructure.Provider;

namespace webapi.Config.Dependancy
{
    public class SqlConnectionDependencySetup : IDependencySetup
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;

        public SqlConnectionDependencySetup(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("ReactDotNetContext");
        }

        public void Run(IServiceCollection services)
        {
            SetupFactories(services);
            SetupRepositories(services);
            SetupQueryServices(services);
            SetupApplicationServices(services);
            SetupDomainServices(services);
        }

        private void SetupFactories(IServiceCollection services)
        {
            services.AddTransient<IUserFactory, SqlUserFactory>();
        }

        private void SetupRepositories(IServiceCollection services)
        {
            if(_connectionString != null)
            {
                services.AddScoped(_ => new DatabaseConnectionProvider(_connectionString));
                services.AddTransient<IUserRepository, SqlUserRepository>();
            }
        }

        private void SetupQueryServices(IServiceCollection services)
        {
            
        }

        private void SetupApplicationServices(IServiceCollection services)
        {
            services.AddTransient<UserApplicationService>();
        }

        private void SetupDomainServices(IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
        }
    }
}