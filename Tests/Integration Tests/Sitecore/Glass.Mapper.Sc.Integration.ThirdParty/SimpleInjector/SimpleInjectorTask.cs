using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.Ioc;
using SimpleInjector;
using LazyObjectInterceptor = Glass.Mapper.Sc.CastleWindsor.Pipelines.ObjectConstruction.LazyObjectInterceptor;

namespace Glass.Mapper.Sc.Integration.ThirdParty.SimpleInjector
{
    public class SimpleInjectorTask : IocTaskBase
    {
        public static Container Container { get;  set; }
       

        protected override bool IsRegistered(Type type)
        {
            return Container.GetRegistration(type) == null;
        }

        protected override void Register(Type type)
        {
            Container.Register(type);
        }

        protected override object CreateConcreteInstance(AbstractTypeConfiguration config)
        {
            return Container.GetInstance(config.Type);
        }

        protected override object[] GetConstructorParameters(AbstractTypeConfiguration config)
        {
            var parameters = config.ConstructorMethods.Select(x => x.Key.GetParameters())
                        .OrderBy(x => x.Length)
                        .First();

            var resolved = parameters.Select(x => Container.GetInstance(x.ParameterType));
            return resolved.ToArray();
        }
    }
}
