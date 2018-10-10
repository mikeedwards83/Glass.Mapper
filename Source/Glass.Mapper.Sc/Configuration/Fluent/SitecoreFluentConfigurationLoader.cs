using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Sc.Configuration.Fluent
{
    /// <summary>
    /// Class SitecoreFluentConfigurationLoader
    /// </summary>
    public class SitecoreFluentConfigurationLoader : IConfigurationLoader
    {
        List<ISitecoreType> _types = new List<ISitecoreType>();

        /// <summary>
        /// Adds the specified config.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config">The config.</param>
        public void Add<T>(SitecoreType<T> config)
        {
            _types.Add(config);
        }

        /// <summary>
        /// Adds this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>SitecoreType{``0}.</returns>
        public SitecoreType<T> Add<T>()
        {
            var config = new SitecoreType<T>();
            _types.Add(config);
            return config;
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        /// <returns>IEnumerable{AbstractTypeConfiguration}.</returns>
        public IEnumerable<AbstractTypeConfiguration> Load()
        {
            return _types.Select(x => x.Config);
        }
    }
}




