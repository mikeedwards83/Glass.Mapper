using System;
using Glass.Mapper.Caching;
using Glass.Mapper.Diagnostics;
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
        public IConfigFactory<AbstractDataMapperResolverTask> DataMapperResolverFactory { get; set; }
        public IConfigFactory<AbstractDataMapper> DataMapperFactory { get; set; }
        public IConfigFactory<AbstractConfigurationResolverTask> ConfigurationResolverFactory { get; set; }
        public IConfigFactory<AbstractObjectConstructionTask> ObjectConstructionFactory { get; set; }
        public IConfigFactory<AbstractObjectSavingTask> ObjectSavingFactory { get; set; }
        public IConfigFactory<ISitecoreQueryParameter> QueryParameterFactory { get; set; }
        public IGlassHtmlFactory GlassHtmlFactory { get; set; }
        public IConfigFactory<IGlassMap> ConfigurationMapFactory { get; set; }
        public abstract ModelCounter GetModelCounter();


        public virtual void Finalise()
        {

            TryFinalise(QueryParameterFactory);
            TryFinalise(DataMapperResolverFactory);
            TryFinalise(DataMapperFactory);
            TryFinalise(ConfigurationResolverFactory);
            TryFinalise(ObjectConstructionFactory);
            TryFinalise(ObjectSavingFactory);
            TryFinalise(ConfigurationMapFactory);
        }

        protected virtual void TryFinalise<T>(IConfigFactory<T> factory)
        {
            var finalisable = factory as AbstractFinalisedConfigFactory<T>;
            if (finalisable != null)
            {
                finalisable.Finalise();
            }
        }


        public abstract ICacheManager GetCacheManager();

        public IItemVersionHandler ItemVersionHandler { get; set; }

        public abstract Mapper.Config GetConfig();
        public abstract ILog GetLog();
    }
}
