using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Glass.Mapper.Configuration.Attributes
{
    public abstract class LinkedAttribute : AbstractPropertyAttribute
    {
        public LinkedAttribute()
        {
            IsLazy = true;
        }
        /// <summary>
        /// Indicates if linked items should be loaded lazily. Default value is true. If false linked items will be loaded when the contain object is created.
        /// </summary>
        public bool IsLazy
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates the type should be inferred from the item template
        /// </summary>
        public bool InferType { get; set; }

        public void Configure(PropertyInfo info, LinkedConfiguration config)
        {
            config.IsLazy = this.IsLazy;
            config.InferType = this.InferType;

            base.Configure(info, config);
        }
    }
}
