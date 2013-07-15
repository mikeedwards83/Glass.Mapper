using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateLazy
{
    public class CreateLazyTask: IObjectConstructionTask
    {
        private static volatile ProxyGenerator _generator;
        private static volatile ProxyGenerationOptions _options;

        static CreateLazyTask()
        {
            _generator = new ProxyGenerator();
            var hook = new LazyObjectProxyHook();
            _options = new ProxyGenerationOptions(hook);
        }
        public void Execute(ObjectConstructionArgs args)
        {
            if (args.AbstractTypeCreationContext.IsLazy)
            {
                //here we create a lazy loaded version of the class
                args.Result = CreateLazyObject(args);
                args.AbortPipeline();

            }
        }

        /// <summary>
        /// Creates the lazy object.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>System.Object.</returns>
        protected virtual object CreateLazyObject(ObjectConstructionArgs args)
        {
            return _generator.CreateClassProxy(args.Configuration.Type, new LazyObjectInterceptor(args));
        }
    }
}
