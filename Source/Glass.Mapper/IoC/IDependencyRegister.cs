using System;

namespace Glass.Mapper.IoC
{
    public interface IDependencyRegister
    {
        string Key { get; }

        Action<IDependencyRegistrar> Action { get; }
    }
}
