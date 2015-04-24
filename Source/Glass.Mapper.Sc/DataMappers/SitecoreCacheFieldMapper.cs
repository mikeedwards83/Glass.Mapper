using System;
using System.Linq;
using Glass.Mapper.Caching;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreCacheFieldMapper : AbstractSitecoreFieldMapper
    {
        /// <summary>
        /// Gets the mapper.
        /// </summary>
        /// <value>The mapper.</value>
        public AbstractDataMapper Mapper { get; private set; }

        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            throw new NotSupportedException("You cannot map through the cache field mapper");
        }

        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            Mapper.MapToCms(mappingContext);
        }

        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            SitecoreTypeCreationDataMappingContext context = mappingContext as SitecoreTypeCreationDataMappingContext;

            if (DisableCache.Current == CacheSetting.Disabled || context == null || !context.TypeCreationContext.CacheEnabled)
            {
                return Mapper.MapToProperty(mappingContext);
            }

            var key = context.Service.GlassContext.Name + context.TypeCreationContext.GetUniqueKey() + Configuration.PropertyInfo.Name;

            ICacheManager cacheManager = context.Service.GlassContext.DependencyResolver.GetCacheManager();
            if (cacheManager == null)
            {
                return Mapper.MapToProperty(mappingContext);
            }

            object result;
            if (!cacheManager.Contains(key))
            {
                result = Mapper.MapToProperty(mappingContext);
                cacheManager.AddOrUpdate(key, result);
            }
            else
            {
                result = cacheManager.Get<object>(key);
            }

            return result;
        }

        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            throw new NotSupportedException("GO AWAY!");
        }

        public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration.Cacheable;
        }

        public override void Setup(DataMapperResolverArgs args)
        {
            base.Setup(args);

            var scConfig = Configuration as SitecoreFieldConfiguration;

            var property = args.PropertyConfiguration.PropertyInfo;
            var type = property.PropertyType;

            if (scConfig == null)
            {
                return;
            }

            var configCopy = scConfig.Copy();
            configCopy.PropertyInfo = new FakePropertyInfo(type, property.Name, property.DeclaringType);

            Mapper =
                args.DataMappers.FirstOrDefault(
                    x => x.CanHandle(configCopy, args.Context) && !(x is SitecoreCacheFieldMapper));


            if (Mapper == null)
                throw new MapperException(
                    "No mapper to handle type {0} on property {1} class {2}".Formatted(type.FullName, property.Name,
                                                                                       property.ReflectedType.FullName));

            Mapper.Setup(new DataMapperResolverArgs(args.Context, configCopy));
        }
    }

    public class SitecoreTypeDataMappingContext
    {
    }
}
