namespace Glass.Mapper.IoC
{
    public interface IDependencyHandler : IDependencyResolver, IDependencyRegistrar
    {
        IGlassInstaller CreateInstaller(Config config);
    }
}
