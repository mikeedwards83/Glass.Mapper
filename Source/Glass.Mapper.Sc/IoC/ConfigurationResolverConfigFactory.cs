using Glass.Mapper.IoC;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.ConfigureOptionsForType;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.StandardResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Pipelines.ConfigurationResolver;

namespace Glass.Mapper.Sc.IoC
{
    public class ConfigurationResolverConfigFactory : AbstractConfigFactory<AbstractConfigurationResolverTask>
    {
        protected DependencyResolver DependencyResolver { get; }

        public ConfigurationResolverConfigFactory(DependencyResolver dependencyResolver)
        {
            DependencyResolver = dependencyResolver;
            Init();
        }

        protected  void Init()
        {
            Add(() => new SitecoreItemResolverTask());
            Add(() => new TemplateInferredTypeTask());
            Add(() => new ConfigurationStandardResolverTask());
            Add(() => new ConfigurationOnDemandResolverTask<SitecoreTypeConfiguration>());
            Add(() => new ConfigureOptionsForType());
        }
    }
}
