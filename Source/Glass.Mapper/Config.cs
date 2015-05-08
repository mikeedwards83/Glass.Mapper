using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper
{
    public class Config 
    {
        /// <summary>
        /// Indicates that classes should be build using the Windsor dependency resolver. Default is False
        /// </summary>
        /// <value>
        /// <c>true</c> if [use windsor contructor]; otherwise, <c>false</c>.
        /// </value>
        public bool UseIoCConstructor { get; set; }
    }
}
