using Glass.Mapper.IoC;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CacheCheck;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateMultiInterface;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.DepthCheck;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.Diagnostics;
using Glass.Mapper.Sc.Caching;
using Glass.Mapper.Sc.Pipelines.ObjectConstruction;
using Sitecore.Diagnostics;

namespace Glass.Mapper.Sc.IoC
{
    public class ObjectConstructionTaskConfigFactory : AbstractFinalisedConfigFactory<AbstractObjectConstructionTask>
    {
        private readonly IDependencyResolver dependencyResolver;

        public ObjectConstructionTaskConfigFactory(IDependencyResolver dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;
            Init();
        }

        protected void Init()
        {
            var config = dependencyResolver.GetConfig();

            //if (config.Debug.Enabled)
            //{
            //    Add(() => new ConstructionCalledMonitorTask());
            //}
            Add(()=> new ModelDepthCheck());
            Add(() => new CreateDynamicTask());
            Add(() => new SitecoreItemTask());
            Add(() => new CacheCheckTask(dependencyResolver.GetCacheManager(), new CacheKeyGenerator()));
            if (config.Debug.Enabled)
            {
                Add(() => new ConstructionTimerStart(new CacheKeyGenerator(), dependencyResolver.GetLog(), dependencyResolver.GetConfig().Debug));
                //Add(() => new ConstructionCreatedMonitorTask());
            }
            Add(() => new EnforcedTemplateCheck());
            Add(() => new CreateMultiInferaceTask());
            Add(() => new CreateConcreteTask());
            Add(() => new CreateInterfaceTask());

        }
    }
}
