using Glass.Mapper.IoC;
using Glass.Mapper.Pipelines.ObjectSaving;
using Glass.Mapper.Pipelines.ObjectSaving.Tasks;

namespace Glass.Mapper.Sc.IoC
{
    public class ObjectSavingTaskConfigFactory : AbstractConfigFactory<AbstractObjectSavingTask>
    {
        protected IDependencyResolver DependencyResolver { get; }

        public ObjectSavingTaskConfigFactory(IDependencyResolver dependencyResolver)
        {
            DependencyResolver = dependencyResolver;
            Init();
        }
        protected void Init()
        {
            Add(() => new StandardSavingTask());
        }
    }
}
