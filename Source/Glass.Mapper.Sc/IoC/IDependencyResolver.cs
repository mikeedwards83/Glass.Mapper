using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.IoC;
using Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters;

namespace Glass.Mapper.Sc.IoC
{
    public interface IDependencyResolver : Glass.Mapper.IoC.IDependencyResolver
    {
        IEnumerable<ISitecoreQueryParameter> QueryParameterFactory();

    }
}
