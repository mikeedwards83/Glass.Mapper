using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.CastleWindsor;

namespace Glass.Mapper.Sc.Integration
{
    public static class Utilities
    {
        public static IDependencyResolver CreateStandardResolver()
        {
            var resolver = DependencyResolver.CreateStandardResolver();
            resolver.Container.Install(new SitecoreInstaller(new Config()));
            return resolver;
        }
    }
}
