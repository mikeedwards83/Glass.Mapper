using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.IoC;

namespace Glass.Mapper
{
    public interface IDependencyHandler : IDependencyResolver, IDependencyRegistrar
    {
        SitecoreInstaller CreateInstaller(Config config);
    }
}
