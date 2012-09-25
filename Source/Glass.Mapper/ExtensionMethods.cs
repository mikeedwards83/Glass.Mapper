using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper
{
    public static class ExtensionMethods
    {
        #region String

        public static string Formatted(this string target, params object[] args)
        {
            return string.Format(target, args);
        }

        public static bool IsNullOrEmpty(this string target)
        {
            return string.IsNullOrEmpty(target);
        }
        public static bool IsNotNullOrEmpty(this string target)
        {
            return !IsNullOrEmpty(target);
        }

        #endregion

        #region IEnumerable<T>
        
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> list, Action<T> action )
        {
            foreach (var item in list)
            {
                action(item);
                yield return item;
            }
        }

        #endregion
    }
}
