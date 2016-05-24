using System;
using Glass.Mapper.Caching;
using Glass.Mapper.IoC;
using Glass.Mapper.Maps;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectSaving;
using Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters;

namespace Glass.Mapper.Sc.IoC
{
    public abstract class AbstractDependencyResolver : IDependencyResolver
    {
        public Mapper.Config Config { get; set; }
        public ILog Log { get; set; }
        public Func<ICacheManager> CacheManager { get; set; }
        public IConfigFactory<IDataMapperResolverTask> DataMapperResolverFactory { get; set; }
        public IConfigFactory<AbstractDataMapper> DataMapperFactory { get; set; }
        public IConfigFactory<IConfigurationResolverTask> ConfigurationResolverFactory { get; set; }
        public IConfigFactory<IObjectConstructionTask> ObjectConstructionFactory { get; set; }
        public IConfigFactory<IObjectSavingTask> ObjectSavingFactory { get; set; }
        public IConfigFactory<ISitecoreQueryParameter> QueryParameterFactory { get; set; }
        public IConfigFactory<IGlassMap> ConfigurationMapFactory { get; set; }

        public abstract ICacheManager GetCacheManager();

        public abstract Mapper.Config GetConfig();
        public abstract ILog GetLog();
    }
}
