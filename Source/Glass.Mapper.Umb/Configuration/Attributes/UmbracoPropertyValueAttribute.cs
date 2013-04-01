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
        /// <param name="propertyValue">The default property value</param>
        public UmbracoPropertyValueAttribute(string propertyAlias, string propertyValue)
        {
            PropertyAlias = propertyAlias;
            PropertyValue = propertyValue;
        }

        /// <summary>
        /// The alias of the property to load
        /// </summary>
        /// <value>
        /// The property alias.
        /// </value>
        public string PropertyAlias { get; set; }

        /// <summary>
        /// The value for the property
        /// </summary>
        /// <value>
        /// The property value.
        /// </value>
        public string PropertyValue { get; set; }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="fieldConfiguration">The field configuration.</param>
        /// <returns></returns>
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
