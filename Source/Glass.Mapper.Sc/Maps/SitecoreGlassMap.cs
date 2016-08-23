using System;
using System.Linq;
using Glass.Mapper.Maps;
using Glass.Mapper.Sc.Configuration.Fluent;

namespace Glass.Mapper.Sc.Maps
{
    /// <summary>
    /// The sitecore specific glass map
    /// </summary>
    /// <typeparam name="T">
    /// The type to map
    /// </typeparam>
    public abstract class SitecoreGlassMap<T> : AbstractGlassMap<SitecoreType<T>, T> where T : class
    {

        /// <summary>
        /// Gets another sitecore type from the configuration
        /// </summary>
        /// <typeparam name="TK">The type to find</typeparam>
        /// <returns></returns>
        protected virtual SitecoreType<TK> GetSitecoreType<TK>() where TK : class
        {
            SitecoreGlassMap<TK> map = MapProvider.Maps.FirstOrDefault(x => x.MappedType == typeof(TK)) as SitecoreGlassMap<TK>;
            if (map == null)
            {
                // todo: unit test this
                throw new Exception(string.Format("Could not find the configuration for type {0}", typeof(TK)));
            }

            return map.GlassType;
        }

        /// <summary>
        /// Imports another map from the configuration
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        protected override void ImportMap<TK>()
        {

            SitecoreType<TK> typeConfig = GetSitecoreType<TK>();
            if (typeConfig == null)
            {
                return;
            }

            GlassType.Import(typeConfig);
        }

        public override void PerformMap<TLoader>(TLoader mappingContainer)
        {
            if (mappingContainer == null)
            {
                return;
            }

            if (GlassType == null)
            {
                return;
            }

            var fluentConfigurationLoader = mappingContainer as SitecoreFluentConfigurationLoader;
            if (fluentConfigurationLoader == null)
            {
                return;
            }

            fluentConfigurationLoader.Add(GlassType);
        }

        /// <summary>
        /// Creates a new blank instance of the type
        /// </summary>
        /// <returns></returns>
        protected override SitecoreType<T> CreateGlassType()
        {
            return new SitecoreType<T>();
        } 
    }
}
