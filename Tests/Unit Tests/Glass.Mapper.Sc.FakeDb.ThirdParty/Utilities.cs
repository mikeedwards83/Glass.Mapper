using Glass.Mapper.Sc.IoC;
using IDependencyResolver = Glass.Mapper.IoC.IDependencyResolver;

namespace Glass.Mapper.Sc.Integration
{
    public static class Utilities
    {
        public static IDependencyResolver CreateStandardResolver()
        {
            Config config = new Config();

            var resolver = new DependencyResolver(config);

            return resolver;
        }
    }
}

