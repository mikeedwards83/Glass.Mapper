using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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
        /// <param name="fieldValue">The default field value </param>
        public SitecoreFieldFieldValueAttribute(string fieldId, string fieldValue)
        {
            FieldValue = fieldValue;
            FieldId = new Guid(fieldId);
        }

        /// <summary>
        /// The Id (Guid) of the field to load
        /// </summary>
        public Guid FieldId { get; set; }

        /// <summary>
        /// The title for the field if using Code First
        /// </summary>
        public string FieldValue { get; set; }

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
