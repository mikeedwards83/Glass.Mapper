using Glass.Mapper.IoC;
using Glass.Mapper.Maps;

namespace Glass.Mapper.Sc.IoC
{
    public class ConfigurationMapConfigFactory : AbstractConfigFactory<IGlassMap>
    {
        protected IDependencyResolver DependencyResolver { get; }

        public ConfigurationMapConfigFactory(IDependencyResolver dependencyResolver)
        {
            DependencyResolver = dependencyResolver;
        }
    }
}
