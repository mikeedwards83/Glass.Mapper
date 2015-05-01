using Glass.Mapper.IoC;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Pipelines.DataMapperResolver.Tasks;

namespace Glass.Mapper.Sc.IoC
{
    public class DataMapperTaskConfigFactory : AbstractConfigFactory<IDataMapperResolverTask>
    {
        protected override void AddTypes()
        {
            Add(() => new DataMapperStandardResolverTask());
        }
    }
}
