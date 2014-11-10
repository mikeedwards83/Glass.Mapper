using System;
using Glass.Mapper.Sc.Configuration.Fluent;

namespace Glass.Mapper.Maps.Sc
{
    /// <summary>
    /// The sitecore specific glass map
    /// </summary>
    /// <typeparam name="T">
    /// The type to map
    /// </typeparam>
    public abstract class SitecoreGlassMap<T> : AbstractGlassMap<SitecoreType<T>>, IGlassMap where T : class
    {
        protected SitecoreGlassMap(IDependencyResolver dependencyResolver) : base(dependencyResolver)
        {
        }

        /// <summary>
        /// Gets another sitecore type from the configuration
        /// </summary>
        /// <typeparam name="TK">The type to find</typeparam>
        /// <returns></returns>
        public SitecoreType<TK> GetSitecoreType<TK>() where TK : class
        {
            IGlassMap<SitecoreType<TK>> sitecoreMap = DependencyResolver.Resolve<IGlassMap<SitecoreType<TK>>>();
            if (sitecoreMap == null)
            {
                // todo: unit test this
                throw new Exception(string.Format("Could not find the configuration for type {0}", typeof(T)));
            }

            return sitecoreMap.GlassType;
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
            if (CanLoad(mappingContainer))
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