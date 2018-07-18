using System;
using System.Reflection;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    /// <summary>
    /// Used to populate default values of a field's field
    /// </summary>
    public class SitecoreFieldFieldValueAttribute : Attribute
    {
        /// <summary>
        /// Indicates that the property should pull data from a Sitecore field.
        /// </summary>
        /// <param name="fieldId">The Id (Guid) of the field to load</param>
        /// <param name="fieldValue">The default field value</param>
        public SitecoreFieldFieldValueAttribute(string fieldId, string fieldValue)
        {
            FieldValue = fieldValue;
            FieldId = new Guid(fieldId);
        }

        /// <summary>
        /// The Id (Guid) of the field to load
        /// </summary>
        /// <value>The field id.</value>
        public Guid FieldId { get; set; }

        /// <summary>
        /// The title for the field if using Code First
        /// </summary>
        /// <value>The field value.</value>
        public string FieldValue { get; set; }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="fieldConfiguration">The field configuration.</param>
        /// <returns>SitecoreFieldFieldValueConfiguration.</returns>
        public SitecoreFieldFieldValueConfiguration Configure(PropertyInfo propertyInfo,
                                                              SitecoreFieldConfiguration fieldConfiguration)
        {
            var config = new SitecoreFieldFieldValueConfiguration();
            
            config.FieldId =  new ID(this.FieldId);
            config.FieldValue = this.FieldValue;
            
            return config;
        }
    }

}

