using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectSaving;

namespace Glass.Mapper
{
    public class PipelineFactory
    {
        public PipelineFactory(Context glassContext)
        {
            ObjectConstructionPipelinePool = new PipelinePool<ObjectConstruction>(glassContext, 1, x => new ObjectConstruction(x.DependencyResolver.ObjectConstructionFactory.GetItems()));
            ConfigurationResolverPipelinePool = new PipelinePool<ConfigurationResolver>(glassContext, 1000, x => new ConfigurationResolver(x.DependencyResolver.ConfigurationResolverFactory.GetItems()));
            ObjectSavingPipelinePool = new PipelinePool<ObjectSaving>(glassContext, 1000, x => new ObjectSaving(x.DependencyResolver.ObjectSavingFactory.GetItems()));
        }

        public IPipelinePool<ObjectConstruction> ObjectConstructionPipelinePool { get; private set; }

        public IPipelinePool<ConfigurationResolver> ConfigurationResolverPipelinePool { get; private set; }

        public IPipelinePool<ObjectSaving> ObjectSavingPipelinePool { get; private set; }
    }
}
