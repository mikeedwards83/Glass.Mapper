using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Umb.Configuration
{
    public class UmbracoInfoConfiguration : InfoConfiguration
    {
        /// <summary>
        /// The type of information that should populate the property
        /// </summary>
        public UmbracoInfoType Type { get; set; }
    }
}
