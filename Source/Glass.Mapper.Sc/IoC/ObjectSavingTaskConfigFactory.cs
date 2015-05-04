using Glass.Mapper.IoC;
using Glass.Mapper.Pipelines.ObjectSaving;
using Glass.Mapper.Pipelines.ObjectSaving.Tasks;

namespace Glass.Mapper.Sc.IoC
{
    public class ObjectSavingTaskConfigFactory : AbstractConfigFactory<IObjectSavingTask>
    {
        protected override void AddTypes()
        {
            Add(() => new StandardSavingTask());
        }
    }
}
