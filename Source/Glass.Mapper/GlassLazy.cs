using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper
{
    public class GlassLazy<T> : Lazy<T>
    {
        public GlassLazy(Func<object> lazy) : base(() => GetValue(lazy))
        {
        }

        private static T GetValue(Func<object> lazy)
        {
            try
            {
                return (T)lazy();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    String.Format("An error occurred whilst attempting to evaluate GlassLazy for type {0}, see the inner exception for details", typeof(T).FullName),
                    ex);
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator T(GlassLazy<T> lazy)
        {
            return lazy != null ? lazy.Value : default(T);
        }

        public static implicit operator GlassLazy<T>(T value)
        {
            return new GlassLazy<T>(() => value);
        }
    }
}
