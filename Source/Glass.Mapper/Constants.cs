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
            public const string ErrorLazyLoop = "Model too deep. Potential lazy loading loop. Type requested: {0}";
        }
    }
}
