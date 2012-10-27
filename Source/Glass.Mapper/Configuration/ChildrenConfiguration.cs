using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Configuration
{
    public class ChildrenConfiguration : AbstractPropertyConfiguration
    {

        /// <summary>
        /// Indicates if children should be loaded lazily. Default value is true. If false all children will be loaded when the containing object is created.
        /// </summary>
        public virtual bool IsLazy
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates the type should be inferred from the item template
        /// </summary>
        public virtual bool InferType
        {
            get;
            set;
        }
    }
}
