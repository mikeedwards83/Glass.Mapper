using Glass.Mapper.Caching;
using Glass.Mapper.Diagnostics;
using Glass.Mapper.Maps;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectSaving;

namespace Glass.Mapper.IoC
{
    /// <summary>
    /// Interface IDependencyResolver
    /// </summary>
    public interface IDependencyResolver
    {
        Config GetConfig();
        ILog GetLog();

        CacheFactory CacheFactory { get; }
        IConfigFactory<AbstractDataMapperResolverTask> DataMapperResolverFactory { get; }
        IConfigFactory<AbstractDataMapper> DataMapperFactory { get; }
        IConfigFactory<AbstractConfigurationResolverTask> ConfigurationResolverFactory { get; }
        IConfigFactory<AbstractObjectConstructionTask> ObjectConstructionFactory { get; }
        IConfigFactory<AbstractObjectSavingTask> ObjectSavingFactory { get; }
        IConfigFactory<IGlassMap> ConfigurationMapFactory { get; }
        
        LazyLoadingHelper LazyLoadingHelper { get; set; }

    }
}




