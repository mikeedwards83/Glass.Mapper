using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Caching
{
    public interface ICacheManager
    {
        object this[string key] { get; set; }
        void ClearCache();
        void Add(string key, object value);
        bool Contains(string key);
    }
}
