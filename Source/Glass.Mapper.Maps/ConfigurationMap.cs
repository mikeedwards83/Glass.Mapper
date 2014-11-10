using System.Linq;
using Glass.Mapper;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Maps
{
    public class ConfigurationMap : IConfigurationMap
    {
        public ConfigurationMap(IDependencyResolver dependencyResolver)
        {
            DependencyResolver = dependencyResolver;
        }

        public IDependencyResolver DependencyResolver { get; private set; }

        /// <summary>
        /// Gets a new instance of a configuration loader using the type, 
        /// DO NOT get a configuration loader and then load it, use one or the other
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetConfigurationLoader<T>() where T : class, IConfigurationLoader, new()
        {
            var loader = new T();
            Load(loader);
            return loader;
        }

        /// <summary>
        /// Loads the configuration into an existing fluent configuration loader
        /// </summary>
        /// <typeparam name="T">The loader type</typeparam>
        /// <param name="loader"></param>
        public void Load<T>(T loader) where T : class, IConfigurationLoader
        {
            IGlassMap[] glassMaps = DependencyResolver.ResolveAll<IGlassMap>().ToArray();
            foreach (IGlassMap sitecoreMap in glassMaps)
            {
                sitecoreMap.PerformMap(loader);
            }
        }
    }
}