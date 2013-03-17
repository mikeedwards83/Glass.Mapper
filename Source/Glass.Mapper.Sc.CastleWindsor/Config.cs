using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.CastleWindsor
{
    public class Config
    {
        public Config()
        {
            UseWindsorContructor = false;
        }
        /// <summary>
        /// Indicates that classes should be build using the Windsor dependency resolver. Default is False
        /// </summary>
        public bool UseWindsorContructor { get; set; }
    }
}
