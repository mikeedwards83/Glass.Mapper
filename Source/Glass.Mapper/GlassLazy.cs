using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper
{
    public class GlassLazy<T> : Lazy<T>
    {
        public GlassLazy(Func<object> lazy) :base(() => (T)lazy())
        {
        }

        public static explicit operator T(GlassLazy<T> lazy)
        {
            return lazy.Value;
        }
    }
}
