using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.IoC;
using IDependencyResolver = Glass.Mapper.IoC.IDependencyResolver;

namespace Glass.Mapper.Sc.V5.Tests
{
    public static class Utilities
    {
        public static IDependencyResolver CreateStandardResolver(Config config = null)
        {
            if (config == null)
                config = new Config();

            var resolver = new DependencyResolver(config);
            return resolver;
        }
    }
}
