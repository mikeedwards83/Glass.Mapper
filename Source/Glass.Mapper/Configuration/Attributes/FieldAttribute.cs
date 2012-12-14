using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Glass.Mapper.Configuration.Attributes
{
    public abstract class FieldAttribute : AbstractPropertyAttribute
    {
        public FieldAttribute()
        { }
        
        /// <summary>
        /// When true the field will not be save back 
        /// </summary>
        public bool ReadOnly { get; set; }

        public void Configure(PropertyInfo propertyInfo, FieldConfiguration config)
        {
            config.ReadOnly = this.ReadOnly;
            base.Configure(propertyInfo, config);
        }
    }
}
