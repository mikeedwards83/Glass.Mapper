using System;
using System.Linq;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.Ioc;
using SimpleInjector;

namespace Glass.Mapper.Sc.FakeDb.ThirdParty.SimpleInjector
{
    public class SimpleInjectorTask : IocTaskBase
    {
        public static Container Container { get;  set; }
       

        protected override bool IsRegistered(Type type)
        {
            return Container.GetCurrentRegistrations().Any(x => x.ServiceType == type);
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
