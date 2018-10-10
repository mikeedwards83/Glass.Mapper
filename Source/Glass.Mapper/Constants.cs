using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper
{
    public class Constants
    {
        public class Errors
        {
            public const string LazyLoop = "Model depth check failed. Model graph too large, enable lazy loading. Type requested: {0}";
            public const string NoDependencyResolver = "No dependency resolver set.";
            public const string OnDemandDisabled = "OnDemand mapping not enabled";
        }
    }
}
