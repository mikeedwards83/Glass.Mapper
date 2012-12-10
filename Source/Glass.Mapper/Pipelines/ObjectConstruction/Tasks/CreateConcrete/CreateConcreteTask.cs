using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.DynamicProxy;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete
{
    public class CreateConcreteTask : IObjectConstructionTask
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
            if (args.Result != null)
                return;

            var type = args.Configuration.Type;

            if(type.IsInterface)
            {
                return;
            }

            if(args.AbstractTypeCreationContext.IsLazy)
            {
                //here we create a lazy loaded version of the class
                args.Result = CreateLazyObject(args);
                args.AbortPipeline();

            }
            else
            {
                //here we create a concrete version of the class
                args.Result = CreateObject(args);
                args.AbortPipeline();

                
            }
        }

        protected virtual object CreateLazyObject(ObjectConstructionArgs args)
        {
            return  _generator.CreateClassProxy(args.Configuration.Type, new LazyObjectInterceptor(args));
        }

        protected virtual object CreateObject(ObjectConstructionArgs args)
        {
            var configuration = args.Configuration;
            var type = configuration.Type;
            var constructorParameters = args.AbstractTypeCreationContext.ConstructorParameters;

            var parameters = 
                constructorParameters == null || !constructorParameters.Any() ? Type.EmptyTypes : constructorParameters.Select(x => x.GetType()).ToArray();

            ConstructorInfo constructor = type.GetConstructor(parameters);

            if (constructor == null)
                throw new ObjectConstructionException(
                    ConstructorErrorMessage.Formatted(type.FullName,parameters
                                                      .Select(x => x.GetType().FullName)
                                                      .Aggregate((x, y) => x + "," + y)));

            Delegate conMethod = args.Configuration.ConstructorMethods[constructor];
            
            var obj = conMethod.DynamicInvoke(parameters);

            //create properties 
            AbstractDataMappingContext dataMappingContext =  args.Service.CreateDataMappingContext(args.AbstractTypeCreationContext, obj);
            args.Configuration.Properties.ForEach(x => x.Mapper.MapCmsToProperty(dataMappingContext));
            return obj;
        }
    }
}
