using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.DynamicProxy;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete
{
    public class CreateConcreteTask : IObjectContructionTask
    {
        private const string ConstructorErrorMessage = "No constructor for class {0} with parameters {1}";

        private static volatile  ProxyGenerator _generator;
        private static volatile  ProxyGenerationOptions _options;

        static CreateConcreteTask()
        {
            _generator = new ProxyGenerator();
            var hook = new LazyObjectProxyHook();
            _options = new ProxyGenerationOptions(hook);
        }

        public void Execute(ObjectConstructionArgs args)
        {
            var type = args.ItemContext.Type;

            if(type.IsInterface)
            {
                return;
            }

            if(args.IsLazy)
            {
                //here we create a lazy loaded version of the class
                args.Object = CreateLazyObject(args);
                args.AbortPipeline = true;

            }
            else
            {
                //here we create a concrete version of the class
                args.Object = CreateObject(args);
                args.AbortPipeline = true;
            }
        }

        protected virtual object CreateLazyObject(ObjectConstructionArgs args)
        {
            return  _generator.CreateClassProxy(args.ItemContext.Type, new LazyObjectInterceptor(args));
        }

        protected virtual object CreateObject(ObjectConstructionArgs args)
        {
            var type = args.ItemContext.Type;

            var parameters = 
                args.ConstructorParameters == null || !args.ConstructorParameters.Any() ? Type.EmptyTypes : args.ConstructorParameters.Select(x => x.GetType()).ToArray();

            ConstructorInfo constructor = type.GetConstructor(parameters);

            if (constructor == null)
                throw new ObjectConstructionException(
                    ConstructorErrorMessage.Formatted(type.FullName,parameters
                                                      .Select(x => x.GetType().FullName)
                                                      .Aggregate((x, y) => x + "," + y)));

            Delegate conMethod = args.ItemContext.ConstructorMethods[constructor];
            
            var obj = conMethod.DynamicInvoke(parameters);

            //map from the CMS to the object
            AbstractDataMappingContext dataContext = new AbstractDataMappingContext();
            dataContext.Object = obj;
            //TODO: setup the context
            args.ItemContext.DataMappers.ForEach(x => x.MapFromCms(dataContext));

            return dataContext.Object;
        }
    }
}
