using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper.Sc.ModelCache
{
    public interface IModelCacheManager
    {
        void Add(string path, Type modelType);

        Type Get(string path);

        string GetKey(string path);
    }
}
