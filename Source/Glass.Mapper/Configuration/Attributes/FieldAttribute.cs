using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Configuration.Attributes
{
    public abstract class FieldAttribute : AbstractPropertyAttribute
    {
        public FieldAttribute()
        { }

        public FieldAttribute(string fieldName)
        {
            FieldName = fieldName;
        }
        /// <summary>
        /// The name of the field  to use if it is different to the property name
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// When true the field will not be save back to Sitecore 
        /// </summary>
        public bool ReadOnly { get; set; }
    }
}
