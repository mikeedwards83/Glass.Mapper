
using Glass.Mapper.Sc.IoC;
using IDependencyResolver = Glass.Mapper.IoC.IDependencyResolver;

namespace Glass.Mapper.Sc.Legacy.FakeDb
{
    public static class Utilities
    {
        public static IDependencyResolver CreateStandardResolver(Config config =null)
        {
            if (config == null)
                config = new Config();

            var resolver = new DependencyResolver(config);
            return resolver;
        }
    }
}

