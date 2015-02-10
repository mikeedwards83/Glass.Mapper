using System;

namespace Glass.Mapper.Sc.IoC
{
    public interface IDependencyInstaller
    {
        string Key { get; }

        Action<IDependencyRegistrar> Action { get; }
    }
}
