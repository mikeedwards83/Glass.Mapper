using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Umb.Configuration
{
    /// <summary>
    /// UmbracoInfoConfiguration
    /// </summary>
    public class UmbracoInfoConfiguration : InfoConfiguration
    {
        /// <summary>
        /// The type of information that should populate the property
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public UmbracoInfoType Type { get; set; }
    }
}
