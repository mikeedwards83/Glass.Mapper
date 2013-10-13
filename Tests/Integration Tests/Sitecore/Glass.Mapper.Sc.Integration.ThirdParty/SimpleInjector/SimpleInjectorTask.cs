using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Glass.Mapper.Sc.CastleWindsor.Pipelines.ObjectConstruction;
using SimpleInjector;

namespace Glass.Mapper.Sc.Integration.ThirdParty.SimpleInjector
{
    public class SimpleInjectorTask : Glass.Mapper.Pipelines.ObjectConstruction.IObjectConstructionTask
    {
        public static Container Container { get;  set; }
        private static object _lock = new object();

         private static volatile  ProxyGenerator _generator;
        private static volatile  ProxyGenerationOptions _options;

        /// <summary>
        /// Initializes static members of the <see cref="CreateConcreteTask"/> class.
        /// </summary>
        static SimpleInjectorTask()
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
                if (Container.GetRegistration(args.Configuration.Type) == null)
                {
                    lock (_lock)
                    {
                        if (Container.GetRegistration(args.Configuration.Type) == null)
                        {
                            Container.Register(args.Configuration.Type);
                        }
                    }

                 
                }

                 Action<object> mappingAction = (target) => 
                        args.Configuration.MapPropertiesToObject(target, args.Service, args.AbstractTypeCreationContext);


                if (args.AbstractTypeCreationContext.IsLazy)
                {
                    var parameters = args.Configuration.ConstructorMethods.Select(x => x.Key.GetParameters())
                        .OrderBy(x => x.Length)
                        .First();

                    var resolved = parameters.Select(x => Container.GetInstance(x.ParameterType));
                    
                    var proxy = _generator.CreateClassProxy(args.Configuration.Type, resolved.ToArray(), new LazyObjectInterceptor( mappingAction));
                    args.Result = proxy;
                }
                else
                {
                    //create instance using SimpleInjector
                    var obj = Container.GetInstance(args.Configuration.Type);

                    //map properties from item to model
                    mappingAction(obj);

                    //set the new object as the returned result
                    args.Result = obj;
                }
            }
        }
    }
}
