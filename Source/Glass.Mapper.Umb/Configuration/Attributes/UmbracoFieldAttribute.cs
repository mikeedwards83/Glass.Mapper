using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Umb.Configuration.Attributes
{
    public class UmbracoFieldAttribute : FieldAttribute
    {
        public UmbracoFieldAttribute(string fieldName)
            : base(fieldName)
        {
        }

        public UmbracoFieldAttribute(int fieldId, UmbracoFieldType fieldType, string fieldTab = "General Properties", bool codeFirst = true)
        {
            FieldId = fieldId;
            FieldTab = fieldTab;
            CodeFirst = codeFirst;
            FieldType = fieldType;
        }

        /// <summary>
        /// The alias for the field
        /// </summary>
        public string FieldAlias { get; set; }

        /// <summary>
        /// The ID of the field when used in a code first scenario 
        /// </summary>
        public int FieldId { get; set; } 

        /// <summary>
        /// Options to override the behaviour of certain fields.
        /// </summary>
        public UmbracoFieldSettings Setting { get; set; }

        #region Code First Properties

        /// <summary>
        /// Indicates the field should be used as part of a code first template
        /// </summary>
        public bool CodeFirst { get; set; }
        
        /// <summary>
        /// The type of field to create when using Code First
        /// </summary>
        public UmbracoFieldType FieldType { get; set; }

        /// <summary>
        /// The name of the tab this field will appear in when using code first.
        /// </summary>
        public string FieldTab { get; set; }

        #endregion

        /// <summary>
        /// Indicates that the property should pull data from a Sitecore field.
        /// </summary>
        public UmbracoFieldAttribute()
        {
        }

        public override AbstractPropertyConfiguration Configure(PropertyInfo propertyInfo)
        {
            var config = new UmbracoFieldConfiguration();
            Configure(propertyInfo, config);
            return config;
        }
        public void Configure(PropertyInfo propertyInfo, UmbracoFieldConfiguration config)
        {
            config.Setting = this.Setting;
            config.CodeFirst = this.CodeFirst;
            
            config.FieldId = this.FieldId;
            config.FieldTab = this.FieldTab;
            config.FieldAlias = this.FieldAlias;
            config.FieldType = this.FieldType;
            config.Setting = this.Setting;
            base.Configure(propertyInfo, config);
        }
    }
}
