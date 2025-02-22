namespace webapi.Config.Dependancy
{
    class DependencySetupFactory
    {
        public IDependencySetup CreateSetup(IConfiguration configuration)
        {
            var setupName = configuration["Dependency:setup"];
            switch (setupName)
            {
                case nameof(SqlConnectionDependencySetup):
                    return new SqlConnectionDependencySetup(configuration);

                default:
                    throw new NotSupportedException(setupName + " is not registered.");
            }
        }
    }
}