using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.Ioc
{
    public abstract class IocTaskBase : Glass.Mapper.Pipelines.ObjectConstruction.IObjectConstructionTask
    {
        private static object _lock = new object();
        private static volatile ProxyGenerator _generator;
        private static volatile ProxyGenerationOptions _options;

        protected static ProxyGenerator Generator { get { return _generator; } }
        protected static ProxyGenerationOptions Options { get { return _options; } }

        static IocTaskBase()
        {
            _generator = new ProxyGenerator();
            var hook = new Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete.LazyObjectProxyHook();
            _options = new ProxyGenerationOptions(hook);
        }

        public void Execute(Glass.Mapper.Pipelines.ObjectConstruction.ObjectConstructionArgs args)
        {
            //check that no other task has created an object
            //also check that this is a dynamic object
            if (args.Result == null && !args.Configuration.Type.IsAssignableFrom(typeof(IDynamicMetaObjectProvider)))
            {
                //check to see if the type is registered with the SimpleInjector container
                //if it isn't added it
                if (IsRegistered(args.Configuration.Type))
                {
                    lock (_lock)
                    {
                        if (IsRegistered(args.Configuration.Type))
                        {
                            Register(args.Configuration.Type);
                        }
                    }


                }

                Action<object> mappingAction = (target) =>
                       args.Configuration.MapPropertiesToObject(target, args.Service, args.AbstractTypeCreationContext);


                if (args.AbstractTypeCreationContext.IsLazy)
                {
                    var resolved = GetConstructorParameters(args.Configuration);

                    var proxy = _generator.CreateClassProxy(args.Configuration.Type, resolved, new LazyObjectInterceptor(mappingAction));
                    args.Result = proxy;
                }
                else
                {
                    //create instance using SimpleInjector
                    var obj = CreateConcreteInstance(args.Configuration);
                    //map properties from item to model
                    mappingAction(obj);

                    //set the new object as the returned result
                    args.Result = obj;
                }
            }
        }

        protected abstract bool IsRegistered(Type type);
        protected abstract void Register(Type type);
        protected abstract object CreateConcreteInstance(AbstractTypeConfiguration config);
        protected abstract object[] GetConstructorParameters(AbstractTypeConfiguration config);
    }
}
