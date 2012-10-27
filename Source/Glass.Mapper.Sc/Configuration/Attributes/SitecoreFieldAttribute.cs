using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    public class SitecoreFieldAttribute : FieldAttribute
    {
        public SitecoreFieldAttribute(string fieldName)
            : base(fieldName)
        {
        }

        /// <summary>
        /// Options to override the behaviour of certain fields.
        /// </summary>
        public SitecoreFieldSettings Setting { get; set; }

        /// <summary>
        /// Indicates that the property should pull data from a Sitecore field.
        /// </summary>
        public SitecoreFieldAttribute()
        {
        }

        public override Mapper.Configuration.AbstractPropertyConfiguration Configure(System.Reflection.PropertyInfo propertyInfo)
        {
            throw new NotImplementedException();
        }
    }
}
