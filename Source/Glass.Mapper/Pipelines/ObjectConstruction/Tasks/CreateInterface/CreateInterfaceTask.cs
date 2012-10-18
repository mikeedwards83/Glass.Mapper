using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface
{
    /// <summary>
    /// Creates classes based on interfaces
    /// </summary>
    public class CreateInterfaceTask : IObjectConstructionTask
    {

        private static volatile  ProxyGenerator _generator;
        private static volatile  ProxyGenerationOptions _options;

        static CreateInterfaceTask()
        {
            _generator = new ProxyGenerator();
            var hook = new CreateInterfaceProxyHook();
            _options = new ProxyGenerationOptions(hook);
        }



        public void Execute(ObjectConstructionArgs args)
        {
            if (args.Result != null)
                return;

            if (args.Configuration.Type.IsInterface)
            {
                args.Result = _generator.CreateInterfaceProxyWithoutTarget(args.Configuration.Type, new InterfacePropertyInterceptor(args));
                args.AbortPipeline();
            }
        }
    }
}
