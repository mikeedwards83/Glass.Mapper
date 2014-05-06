using System.Linq;
using Castle.DynamicProxy;
using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Sc.ContentSearch.Pipelines.ObjectConstruction.Tasks.SearchProxy
{
    public class SearchProxyWrapperTask : IObjectConstructionTask
    {
        private static volatile ProxyGenerator _generator = new ProxyGenerator();

        static SearchProxyWrapperTask()
        {
        }

        public void Execute(ObjectConstructionArgs args)
        {
            if (args.Result != null || !SearchSwitcher.IsSearchContext || args.Configurations.First().Type.IsSealed)
                return;
            if (args.Configurations.First().Type.IsInterface)
            {
                args.Result = _generator.CreateInterfaceProxyWithoutTarget(args.Configurations.First().Type, new SearchInterceptor(args));
                args.AbortPipeline();
            }
            else
            {
                args.Result = _generator.CreateClassProxy(args.Configurations.First().Type,new SearchInterceptor(args));
                args.AbortPipeline();
            }
        }
    }
}
