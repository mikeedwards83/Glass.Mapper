using Glass.Mapper.IoC;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Pipelines.DataMapperResolver.Tasks;

namespace Glass.Mapper.Sc.IoC
{
    public class DataMapperTaskConfigFactory : AbstractConfigFactory<AbstractDataMapperResolverTask>
    {
        protected IDependencyResolver DependencyResolver { get; }

        public DataMapperTaskConfigFactory(IDependencyResolver dependencyResolver)
        {
            DependencyResolver = dependencyResolver;
            Init();
        }

        protected  void Init()
        {
            Add(() => new DataMapperAttributeResolverTask());
            Add(() => new DataMapperStandardResolverTask());
        }
    }
}
