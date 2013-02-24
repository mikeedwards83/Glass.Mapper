﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Umb.Configuration
{
    public class UmbracoLinkedConfiguration : LinkedConfiguration
    {
        /// <summary>
        /// Indicate weather All, References or Referred should be loaded
        /// </summary>
        public UmbracoLinkedOptions Option { get; set; }
    }
}
