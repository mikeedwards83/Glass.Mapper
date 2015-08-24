using Glass.Mapper.IoC;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.MultiInterfaceResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.StandardResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Pipelines.ConfigurationResolver;

namespace Glass.Mapper.Sc.IoC
{
    public class ConfigurationResolverConfigFactory : AbstractConfigFactory<IConfigurationResolverTask>
    {
        public ConfigurationResolverConfigFactory()
        {
            Init();
        }

        protected  void Init()
        {
            Add(() => new SitecoreItemResolverTask());
            Add(() => new MultiInterfaceResolverTask());
            Add(() => new TemplateInferredTypeTask());
            Add(() => new ConfigurationStandardResolverTask());
            Add(() => new ConfigurationOnDemandResolverTask<SitecoreTypeConfiguration>());
        }
    }
}
