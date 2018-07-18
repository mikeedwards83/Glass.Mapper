using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper.Configuration
{
    public enum Cache
    {
        /// <summary>
        /// Default
        /// </summary>
        Default = 0 ,
        
        /// <summary>
        /// Enable the cache 
        /// </summary>
        Enabled = 5,

        /// <summary>
        /// Disable the cache
        /// </summary>
        Disabled = 10,
    }

    public static class CachableExtensions
    {
        public static bool IsEnabled(this Cache value)
        {
            return value == Cache.Enabled;
        }
    }
}
