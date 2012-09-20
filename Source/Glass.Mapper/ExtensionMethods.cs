using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper
{
    public static class ExtensionMethods
    {
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

    }
}
