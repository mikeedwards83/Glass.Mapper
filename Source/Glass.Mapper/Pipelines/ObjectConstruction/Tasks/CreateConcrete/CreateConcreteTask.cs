
using System;
using System.Linq;
using Castle.DynamicProxy;
using Glass.Mapper.Diagnostics;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete
{
    /// <summary>
    /// Class CreateConcreteTask
    /// </summary>
    public class CreateConcreteTask : AbstractObjectConstructionTask
    {
        private readonly LazyLoadingHelper _lazyLoadingHelper;
        private static volatile  ProxyGenerator _generator;

        /// <summary>
        /// Initializes static members of the <see cref="CreateConcreteTask"/> class.
        /// </summary>
        static CreateConcreteTask()
        {
            _generator = new ProxyGenerator();
        }

        public CreateConcreteTask(LazyLoadingHelper lazyLoadingHelper)
        {
            _lazyLoadingHelper = lazyLoadingHelper;
            Name = "CreateConcreteTask";
        }

        /// <summary>
        /// Executes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        public override void Execute(ObjectConstructionArgs args)
        {
            if (args.Result == null
                && args.Configuration != null
                && !args.Configuration.Type.IsInterface
                && !args.Configuration.Type.IsSealed)
            {
                if(_lazyLoadingHelper.IsEnabled(args.Options))
                {
                    //here we create a lazy loaded version of the class
                    args.Result = CreateLazyObject(args);
                    ModelCounter.Instance.ProxyModelsCreated++;
                }
                else
                {
                    //here we create a concrete version of the class
                    args.Result = CreateObjectAndMapProperties(args);
                    ModelCounter.Instance.ModelsMapped++;
                    ModelCounter.Instance.ConcreteModelCreated++;
                }
            }

            base.Execute(args);
        }

        /// <summary>
        /// Creates the lazy object.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>System.Object.</returns>
        protected virtual object CreateLazyObject(ObjectConstructionArgs args)
        {
            var proxy =   _generator.CreateClassProxy(
                args.Configuration.Type, 
                new ProxyGenerationOptions(), 
                args.Options.ConstructorParameters.Select(x=>x.Value).ToArray(), 
                new LazyObjectInterceptor(args, _lazyLoadingHelper));

            args.Configuration.MapPrivatePropertiesToObject(proxy, args.Service, args.AbstractTypeCreationContext);

            return proxy;
        }

        /// <summary>
        /// Creates the object.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>System.Object.</returns>
        protected virtual object CreateObjectAndMapProperties(ObjectConstructionArgs args)
        {
            var constructorParameters = args.Options.ConstructorParameters;

            try
            {
                object obj = CreateConcreteObject(args);
                args.Configuration.MapPropertiesToObject(obj, args.Service, args.AbstractTypeCreationContext);
                return obj;

            }
            catch (MapperStackException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to create type {0}".Formatted(args.Configuration.Type), ex);
            }
        }

        private object CreateConcreteObject(ObjectConstructionArgs args)
        {
            object obj;

            var constructorParameters = args.Options.ConstructorParameters;

            if (constructorParameters == null || !constructorParameters.Any())
            {
                //conMethod = args.Configuration.DefaultConstructor;
                obj = Activator.CreateInstance(args.Configuration.Type);
            }
            else
            {
                var parameterTypes = constructorParameters.Select(x => x.Type).ToArray();
                var parameters = constructorParameters.Select(x => x.Value).ToArray();
                var constructorInfo = args.Configuration.Type.GetConstructor(parameterTypes);
                var conMethod = args.Configuration.ConstructorMethods[constructorInfo];
                obj = conMethod.DynamicInvoke(parameters);
            }
            return obj;
        }
    }
}




