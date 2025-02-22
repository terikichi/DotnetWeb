namespace webapi.Config.Dependancy
{
    public interface IDependencySetup
    {
        void Run(IServiceCollection services);
    }
}