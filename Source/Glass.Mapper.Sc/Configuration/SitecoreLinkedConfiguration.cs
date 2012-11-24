using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Sc.Configuration
{
    public class SitecoreLinkedConfiguration : LinkedConfiguration
    {
        /// <summary>
        /// Indicate weather All, References or Referred should be loaded
        /// </summary>
        public SitecoreLinkedOptions Option { get; set; }
    }
}
