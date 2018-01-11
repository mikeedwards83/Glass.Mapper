using Glass.Mapper.IoC;
using Glass.Mapper.Pipelines.ObjectSaving;
using Glass.Mapper.Pipelines.ObjectSaving.Tasks;

namespace Glass.Mapper.Sc.IoC
{
    public class ObjectSavingTaskConfigFactory : AbstractFinalisedConfigFactory<AbstractObjectSavingTask>
    {
        public ObjectSavingTaskConfigFactory()
        {
            Init();
        }
        protected void Init()
        {
            Add(() => new StandardSavingTask());
        }
    }
}
