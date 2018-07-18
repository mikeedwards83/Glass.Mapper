using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper
{
    public class LazyLoadingHelper
    {
        /// <summary>
        /// Checks to see if lazy loading has been enabled for the object construction.
        /// </summary>
        /// <param name="options">The options being used to build the model.</param>
        /// <returns></returns>
        public virtual bool IsEnabled(GetOptions options )
        {
            return options.Lazy == LazyLoading.Enabled;
        }
    }
}
