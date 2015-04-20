using System.Linq;
using Glass.Mapper.Configuration;
using Glass.Mapper.IoC;

namespace Glass.Mapper.Maps
{
    public class ConfigurationMap : IConfigurationMap, IMapProvider
    {
        public ConfigurationMap(IGlassMap[] maps)
        {
            Maps = maps;
        }

        public ConfigurationMap(IDependencyResolver dependencyResolver)
        {
            if (dependencyResolver.ConfigurationMapFactory == null || dependencyResolver.ConfigurationMapFactory.GetItems() == null)
            {
                return;
            }

            Maps = dependencyResolver.ConfigurationMapFactory.GetItems().ToArray();
        }

        public IGlassMap[] Maps { get; private set; }

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
            if (Maps == null)
            {
                return;
            }

            foreach (IGlassMap sitecoreMap in Maps)
            {
                sitecoreMap.SetMapProvider(this);
            }

            foreach (IGlassMap sitecoreMap in Maps)
            {
                sitecoreMap.PerformMap(loader);
            }
        }
    }
}