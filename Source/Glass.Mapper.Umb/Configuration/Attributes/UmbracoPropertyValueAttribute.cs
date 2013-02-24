using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Glass.Mapper.Umb.Configuration.Attributes
{
    /// <summary>
    /// Used to populate default values of a property
    /// </summary>
    public class UmbracoPropertyValueAttribute : Attribute
    {
        /// <summary>
        /// Indicates that the property should pull data from a umbraco property.
        /// </summary>
        /// <param name="propertyAlias">The alias of the property to load</param>
        /// <param name="propertyValue">The default property value </param>
        public UmbracoPropertyValueAttribute(string propertyAlias, string propertyValue)
        {
            PropertyAlias = propertyAlias;
            PropertyValue = propertyValue;
        }

        /// <summary>
        /// The alias of the property to load
        /// </summary>
        public string PropertyAlias { get; set; }

        /// <summary>
        /// The value for the property 
        /// </summary>
        public string PropertyValue { get; set; }

        public UmbracoPropertyValueConfiguration Configure(PropertyInfo propertyInfo,
                                                              UmbracoPropertyConfiguration fieldConfiguration)
        {
            var config = new UmbracoPropertyValueConfiguration
                {
                    PropertyAlias = this.PropertyAlias,
                    PropertyValue = this.PropertyValue
                };

            return config;
        }
    }
}
