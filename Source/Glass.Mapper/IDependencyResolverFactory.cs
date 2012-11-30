using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper
{
    public interface IDependencyResolverFactory
    {
        IDependencyResolver GetResolver();
    }
}
