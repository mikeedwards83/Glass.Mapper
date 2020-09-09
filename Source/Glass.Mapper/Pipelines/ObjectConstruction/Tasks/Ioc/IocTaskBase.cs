using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using Glass.Mapper.Configuration;
using Glass.Mapper.Diagnostics;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.Ioc
{
    public abstract class IocTaskBase : Glass.Mapper.Pipelines.ObjectConstruction.AbstractObjectConstructionTask
    {
        private readonly LazyLoadingHelper _lazyLoadingHelper;
        private static object _lock = new object();
        private static readonly ProxyGenerator _generator;
        private static readonly ProxyGenerationOptions _options;

        protected static ProxyGenerator Generator { get { return _generator; } }
        protected static ProxyGenerationOptions Options { get { return _options; } }

        static IocTaskBase()
        {
            _generator = new ProxyGenerator();
            var hook = new Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete.LazyObjectProxyHook();
            _options = new ProxyGenerationOptions(hook);
        }

        protected IocTaskBase(LazyLoadingHelper lazyLoadingHelper)
        {
            _lazyLoadingHelper = lazyLoadingHelper;
            Name = "IocTaskBase";
        }

        public override void Execute(Glass.Mapper.Pipelines.ObjectConstruction.ObjectConstructionArgs args)
        {
            //check that no other task has created an object
            //also check that this is a dynamic object


            if (args.Result == null)
            {
                var configuration = args.Configuration;
                if(!configuration.Type.IsAssignableFrom(typeof(IDynamicMetaObjectProvider)))
                {
                    //check to see if the type is registered with the SimpleInjector container
                    //if it isn't added it
                    if (!IsRegistered(configuration.Type))
                    {
                        lock (_lock)
                        {
                            if (!IsRegistered(configuration.Type))
                            {
                                Register(configuration.Type);
                            }
                        }


                    }

                    Action<object> mappingAction = (target) =>
                                                   configuration.MapPropertiesToObject(target, args.Service,
                                                                                            args
                                                                                                .AbstractTypeCreationContext);


                    if (_lazyLoadingHelper.IsEnabled(args.Options))
                    {
                        var resolved = GetConstructorParameters(configuration);

                        var proxy = _generator.CreateClassProxy(configuration.Type, resolved,
                                                                new LazyObjectInterceptor(mappingAction, args));
                        args.Result = proxy;
                        ModelCounter.Instance.ProxyModelsCreated++;
                    }
                    else
                    {
                        //create instance using SimpleInjector
                        var obj = CreateConcreteInstance(configuration);
                        //map properties from item to model
                        mappingAction(obj);

                        //set the new object as the returned result
                        args.Result = obj;
                        ModelCounter.Instance.ConcreteModelCreated++;
                        ModelCounter.Instance.ModelsMapped++;

                    }
                }
            }

            base.Execute(args);
        }

        protected abstract bool IsRegistered(Type type);
        protected abstract void Register(Type type);
        protected abstract object CreateConcreteInstance(AbstractTypeConfiguration config);
        protected abstract object[] GetConstructorParameters(AbstractTypeConfiguration config);
    }
}
