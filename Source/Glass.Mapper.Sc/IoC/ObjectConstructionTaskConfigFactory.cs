using System;
using Glass.Mapper.Caching;
using Glass.Mapper.IoC;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CacheAdd;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CacheCheck;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateMultiInterface;
using Glass.Mapper.Sc.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Sc.IoC
{
    public class ObjectConstructionTaskConfigFactory : AbstractConfigFactory<IObjectConstructionTask>
    {
        private readonly Func<ICacheManager> cacheManager;

        public ObjectConstructionTaskConfigFactory(Func<ICacheManager> cacheManager)
        {
            this.cacheManager = cacheManager;
        }

        protected override void AddTypes()
        {
            Add(() => new CreateDynamicTask());
            Add(() => new SitecoreItemTask());
            Add(() => new CacheCheckTask(cacheManager()));
            Add(() => new EnforcedTemplateCheck());
            Add(() => new CreateMultiInferaceTask());
            Add(() => new CreateConcreteTask());
            Add(() => new CreateInterfaceTask());
            Add(() => new CacheAddTask(cacheManager()));
        }
    }
}
