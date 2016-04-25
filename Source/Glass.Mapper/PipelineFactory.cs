using Glass.Mapper.IoC;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectSaving;

namespace Glass.Mapper
{
    public class PipelineFactory
    {
        public PipelineFactory(IDependencyResolver dependencyResolver)
        {
            ObjectConstructionPipelinePool = new PipelinePool<ObjectConstruction>(dependencyResolver, 500, x => new ObjectConstruction(x.ObjectConstructionFactory.GetItems()));
            ConfigurationResolverPipelinePool = new PipelinePool<ConfigurationResolver>(dependencyResolver, 500, x => new ConfigurationResolver(x.ConfigurationResolverFactory.GetItems()));
            ObjectSavingPipelinePool = new PipelinePool<ObjectSaving>(dependencyResolver, 500, x => new ObjectSaving(x.ObjectSavingFactory.GetItems()));
            DataMapperResolverPipelinePool = new PipelinePool<DataMapperResolver>(dependencyResolver, 250, x => new DataMapperResolver(x.DataMapperResolverFactory.GetItems()));
        }

        public IPipelinePool<ObjectConstruction> ObjectConstructionPipelinePool { get; private set; }

        public IPipelinePool<ConfigurationResolver> ConfigurationResolverPipelinePool { get; private set; }

        public IPipelinePool<DataMapperResolver> DataMapperResolverPipelinePool { get; private set; }

        public IPipelinePool<ObjectSaving> ObjectSavingPipelinePool { get; private set; }
    }
}
