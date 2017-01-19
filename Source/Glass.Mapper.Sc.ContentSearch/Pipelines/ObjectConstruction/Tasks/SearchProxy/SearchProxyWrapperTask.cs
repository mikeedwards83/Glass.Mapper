using System.Linq;
using Castle.DynamicProxy;
using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Sc.ContentSearch.Pipelines.ObjectConstruction.Tasks.SearchProxy
{
    public class SearchProxyWrapperTask : AbstractObjectConstructionTask
    {
        private static volatile ProxyGenerator _generator = new ProxyGenerator();

        static SearchProxyWrapperTask()
        {
        }

        public SearchProxyWrapperTask()
        {
            Name = "SearchProxyWrapperTask";
        }
        public override void Execute(ObjectConstructionArgs args)
        {
            if (args.Result != null || !SearchSwitcher.IsSearchContext || args.Configuration.Type.IsSealed)
            {
                base.Execute(args);
                return;
            }

            if (args.Configuration.Type.IsInterface)
            {
                args.Result = _generator.CreateInterfaceProxyWithoutTarget(args.Configuration.Type, new SearchInterceptor(args));
            }
            else
            {
                args.Result = _generator.CreateClassProxy(args.Configuration.Type,new SearchInterceptor(args));
            }

            if (args.Result == null)
            {
                base.Execute(args);
            }
        }
    }
}
