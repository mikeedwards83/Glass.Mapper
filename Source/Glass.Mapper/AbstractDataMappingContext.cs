using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper
{
    /// <summary>
    /// Represents the context when a CMS value is mapper to/from a .Net property value
    /// </summary>
    public class AbstractDataMappingContext
    {
        public AbstractDataMappingContext(object obj)
        {
            this.Object = obj;
        }

        /// <summary>
        /// Value stored by the CMS
        /// </summary>
        public string CmsValue { get; set; }

        /// <summary>
        /// Value stored by the Property
        /// </summary>
        public object PropertyValue { get; set; }

        /// <summary>
        /// The object containing the property being mapped
        /// </summary>
        public object Object { get; private set; }
    }
}
