using Glass.Mapper.IoC;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Pipelines.DataMapperResolver.Tasks;

namespace Glass.Mapper.Sc.IoC
{
    public class DataMapperTaskConfigFactory : AbstractFinalisedConfigFactory<AbstractDataMapperResolverTask>
    {
        public DataMapperTaskConfigFactory()
        {
            Init();
        }

        protected  void Init()
        {
            Add(() => new DataMapperAttributeResolverTask());
            Add(() => new DataMapperStandardResolverTask());
        }
    }
}
