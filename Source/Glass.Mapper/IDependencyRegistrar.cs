using System;

namespace Glass.Mapper
{
    public interface IDependencyRegistrar
    {
        void RegisterTransient<T, TComponent>() where T : class;

        void RegisterTransient(Type type);

        void RegisterInstance<T>(T instance) where T : class;
    }
}
