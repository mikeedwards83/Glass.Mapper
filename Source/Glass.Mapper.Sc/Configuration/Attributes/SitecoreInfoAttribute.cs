using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    public class SitecoreInfoAttribute : InfoAttribute
    {
        /// <summary>
        /// The type of information that should populate the property
        /// </summary>
        public SitecoreInfoType Type { get; set; }

        /// <summary>
        /// UrlOptions, use in conjunction with SitecoreInfoType.Url
        /// </summary>
        public SitecoreInfoUrlOptions UrlOptions { get; set; }
    }
}
