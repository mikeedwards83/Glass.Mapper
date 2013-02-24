using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Umb.Configuration
{
    /// <summary>
    /// Used to populate default values of a property
    /// </summary>
    public class UmbracoPropertyValueConfiguration
    {
        /// <summary>
        /// The alias of the property to load
        /// </summary>
        public string PropertyAlias { get; set; }

        /// <summary>
        /// The value for the property if using Code First
        /// </summary>
        public string PropertyValue { get; set; }
    }
}
