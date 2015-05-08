using Glass.Mapper.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper.Sc.CastleWindsor.Tests
{
    public class Utilities
    {
        public static IDependencyResolver CreateStandardResolver(bool useWindsorContainer = false)
        {
            var resolver = DependencyResolver.CreateStandardResolver();
            var config = new Config { UseIoCConstructor = useWindsorContainer };
            WindsorSitecoreInstaller sitecoreInstaller = new WindsorSitecoreInstaller(config);
            resolver.Container.Install(sitecoreInstaller);
            return resolver;
        }
    }
}
