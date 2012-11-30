using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.CastleWindsor
{
    public class CastleDependencyResolverFactory : IDependencyResolverFactory
    {
        public IDependencyResolver GetResolver()
        {
            return new CastleDependencyResolver();
        }
    }
}
