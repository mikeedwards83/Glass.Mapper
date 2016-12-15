using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper.Sc.Pipelines.Response
{
    public interface ITypeFinder
    {
        Type GetType(string path);
    }
}
