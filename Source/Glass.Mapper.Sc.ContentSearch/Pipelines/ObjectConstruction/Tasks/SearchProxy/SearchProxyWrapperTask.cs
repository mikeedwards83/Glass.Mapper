using System.Linq;
using Castle.DynamicProxy;
using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Sc.ContentSearch.Pipelines.ObjectConstruction.Tasks.SearchProxy
{
    public class SearchProxyWrapperTask : IObjectConstructionTask
    {
        private static volatile ProxyGenerator _generator = new ProxyGenerator();

        public void Execute(ObjectConstructionArgs args)
        {
            if (args.Result != null || !SearchSwitcher.IsSearchContext || args.Configuration.Type.IsSealed)
                return;
            if (args.Configuration.Type.IsInterface)
            {
                args.Result = _generator.CreateInterfaceProxyWithoutTarget(args.Configuration.Type, new SearchInterceptor(args));
                args.AbortPipeline();
            }
            else
            {
                args.Result = _generator.CreateClassProxy(args.Configuration.Type,new SearchInterceptor(args));
                args.AbortPipeline();
            }
        }
    }
}
