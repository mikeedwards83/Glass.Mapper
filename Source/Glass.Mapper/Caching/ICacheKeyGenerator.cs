using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Caching
{
    public interface ICacheKeyGenerator
    {
        string Generate(ObjectConstructionArgs args);
    }
}
