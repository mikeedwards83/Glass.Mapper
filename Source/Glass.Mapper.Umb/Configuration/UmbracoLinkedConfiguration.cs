using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Umb.Configuration
{
    /// <summary>
    /// UmbracoLinkedConfiguration
    /// </summary>
    public class UmbracoLinkedConfiguration : LinkedConfiguration
    {
        /// <summary>
        /// Indicate weather All, References or Referred should be loaded
        /// </summary>
        /// <value>
        /// The option.
        /// </value>
        public UmbracoLinkedOptions Option { get; set; }
    }
}
