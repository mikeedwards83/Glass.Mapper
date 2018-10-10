using Glass.Mapper.IoC;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CacheCheck;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.DepthCheck;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.Diagnostics;
using Glass.Mapper.Sc.Caching;
using Glass.Mapper.Sc.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Sc.IoC
{
    public class ObjectConstructionTaskConfigFactory : AbstractConfigFactory<AbstractObjectConstructionTask>
    {
        protected  IDependencyResolver DependencyResolver { get; }

        public ObjectConstructionTaskConfigFactory(IDependencyResolver dependencyResolver)
        {
            DependencyResolver = dependencyResolver;
            Init();
        }

        protected void Init()
        {
            var config = DependencyResolver.GetConfig();

            //if (config.Debug.Enabled)
            //{
            //    Add(() => new ConstructionCalledMonitorTask());
            //}
            
            Add(()=> new NullItemTask());
            Add(()=> new ModelDepthCheck());
            Add(()=>  new ItemVersionCountByRevisionTask());
            Add(() => new SitecoreItemTask());
            Add(() => new CacheCheckTask(DependencyResolver.GetCacheManager(), new CacheKeyGenerator()));
            if (config.Debug.Enabled)
            {
                Add(() => new ConstructionTimerStart(new CacheKeyGenerator(), DependencyResolver.GetLog(), DependencyResolver.GetConfig().Debug));
                //Add(() => new ConstructionCreatedMonitorTask());
            }
            Add(() => new EnforcedTemplateCheck());
            Add(() => new CreateConcreteTask(DependencyResolver.LazyLoadingHelper));
            Add(() => new CreateInterfaceTask(DependencyResolver.LazyLoadingHelper));

        }
    }
}
