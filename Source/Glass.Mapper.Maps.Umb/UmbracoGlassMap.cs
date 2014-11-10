using System;
using Glass.Mapper.Umb.Configuration.Fluent;

namespace Glass.Mapper.Maps.Umb
{
    /// <summary>
    /// The umbraco specific glass map
    /// </summary>
    /// <typeparam name="T">
    /// The type to map
    /// </typeparam>
    public abstract class UmbracoGlassMap<T> : AbstractGlassMap<UmbracoType<T>> where T : class
    {
        protected UmbracoGlassMap(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        {
        }

        /// <summary>
        /// Gets another umbraco type from the configuration
        /// </summary>
        /// <typeparam name="TK">The type to find</typeparam>
        /// <returns></returns>
        public UmbracoType<TK> GetUmbracoType<TK>() where TK : class
        {
            IGlassMap<UmbracoType<TK>> umbracoMap = DependencyResolver.Resolve<IGlassMap<UmbracoType<TK>>>();
            if (umbracoMap == null)
            {
                // todo: unit test this
                throw new Exception(string.Format("Could not find the configuration for type {0}", typeof(T)));
            }

            return umbracoMap.GlassType;
        }

        /// <summary>
        /// Performs the mapping against the container
        /// </summary>
        /// <param name="mappingContainer">
        /// The mapping container - expects an UmbracoFluentConfigurationLoader</param>
        public override void PerformMap<TLoader>(TLoader mappingContainer)
        {
            if (CanLoad(mappingContainer))
            {
                return;
            }

            var fluentConfigurationLoader = mappingContainer as UmbracoFluentConfigurationLoader;
            if (fluentConfigurationLoader == null)
            {
                return;
            }

            fluentConfigurationLoader.Add(GlassType);
        }

        /// <summary>
        /// Imports another map from the configuration
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        protected override void ImportMap<TK>()
        {
            UmbracoType<TK> typeConfig = GetUmbracoType<TK>();
            if (typeConfig == null)
            {
                return;
            }

            GlassType.Import(typeConfig);
        }

        /// <summary>
        /// Creates a new blank instance of the type
        /// </summary>
        /// <returns></returns>
        protected override UmbracoType<T> CreateGlassType()
        {
            return new UmbracoType<T>();
        }
    }
}