using System.Linq;
using System.Reflection;
using Glass.Mapper.Configuration;
using Glass.Mapper.Configuration.Attributes;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    /// <summary>
    /// Class SitecoreFieldAttribute
    /// </summary>
    public class SitecoreFieldAttribute : FieldAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreFieldAttribute"/> class.
        /// </summary>
        public SitecoreFieldAttribute()
        {
            Setting = SitecoreFieldSettings.Default;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreFieldAttribute"/> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        public SitecoreFieldAttribute(string fieldName) : this()
        {
            FieldName = fieldName;
            FieldSortOrder = -1;
            SectionSortOrder = -1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreFieldAttribute"/> class.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <param name="codeFirst">if set to <c>true</c> [code first].</param>
        public SitecoreFieldAttribute(string fieldId, SitecoreFieldType fieldType, string sectionName = "Data", bool codeFirst = true)
        {
            FieldId = fieldId;
            SectionName = sectionName;
            CodeFirst = codeFirst;
            FieldType = fieldType;
            FieldSortOrder = -1;
            SectionSortOrder = 100;
        }

        /// <summary>
        /// Use with the Glass.Mapper.Sc.Fields.Link type
        /// </summary>
        /// <value>The URL options.</value>
        public SitecoreInfoUrlOptions UrlOptions { get; set; }

        /// <summary>
        /// The name of the field  to use if it is different to the property name
        /// </summary>
        /// <value>The name of the field.</value>
        public string FieldName { get; set; }

        /// <summary>
        /// The ID of the field when used in a code first scenario
        /// </summary>
        /// <value>The field id.</value>
        public string FieldId { get; set; }

        /// <summary>
        /// Options to override the behaviour of certain fields.
        /// </summary>
        /// <value>The setting.</value>
        public SitecoreFieldSettings Setting { get; set; }

        public SitecoreMediaUrlOptions MediaUrlOptions { get; set; }

        #region Code First Properties

        /// <summary>
        /// Indicates the field should be used as part of a code first template
        /// </summary>
        /// <value><c>true</c> if [code first]; otherwise, <c>false</c>.</value>
        public bool CodeFirst { get; set; }

        /// <summary>
        /// The type of field to create when using Code First
        /// </summary>
        /// <value>The type of the field.</value>
        public SitecoreFieldType FieldType { get; set; }

        /// <summary>
        /// The name of the section this field will appear in when using code first.
        /// </summary>
        /// <value>The name of the section.</value>
        public string SectionName { get; set; }

        /// <summary>
        /// The title for the field if using Code First
        /// </summary>
        /// <value>The field title.</value>
        public string FieldTitle { get; set; }

        /// <summary>
        /// The source for the field if using Code First
        /// </summary>
        /// <value>The field source.</value>
        public string FieldSource { get; set; }

        /// <summary>
        /// Sets the field as shared if using Code First
        /// </summary>
        /// <value><c>true</c> if this instance is shared; otherwise, <c>false</c>.</value>
        public bool IsShared { get; set; }

        /// <summary>
        /// Sets the field as unversioned if using Code First
        /// </summary>
        /// <value><c>true</c> if this instance is unversioned; otherwise, <c>false</c>.</value>
        public bool IsUnversioned { get; set; }

        /// <summary>
        /// Overrides the field sort order if using Code First
        /// </summary>
        /// <value>The field sort order.</value>
        public int FieldSortOrder { get; set; }

        /// <summary>
        /// Overrides the section sort order if using Code First
        /// </summary>
        /// <value>The section sort order.</value>
        public int SectionSortOrder { get; set; }

        /// <summary>
        /// Overrides the field validation regular expression if using Code First
        /// </summary>
        /// <value>The validation regular expression.</value>
        public string ValidationRegularExpression { get; set; }

        /// <summary>
        /// Overrides the field validation error text if using Code First
        /// </summary>
        /// <value>The validation error text.</value>
        public string ValidationErrorText { get; set; }

        /// <summary>
        /// Sets the field as required if using Code First
        /// </summary>
        /// <value><c>true</c> if this instance is required; otherwise, <c>false</c>.</value>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Allows for custom types of field to create when using Code First
        /// </summary>
        /// <value>The type of the field.</value>
        public string CustomFieldType { get; set; }

        #endregion



        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns>AbstractPropertyConfiguration.</returns>
        public override AbstractPropertyConfiguration Configure(PropertyInfo propertyInfo)
        {
            var config = new SitecoreFieldConfiguration();
            Configure(propertyInfo, config);
            return config;
        }
        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="config">The config.</param>
        public void Configure(PropertyInfo propertyInfo, SitecoreFieldConfiguration config)
        {
            config.FieldName = this.FieldName;

            if (config.FieldName.IsNullOrEmpty())
                config.FieldName = propertyInfo.Name;

            if(FieldId.HasValue())
                config.FieldId = new ID(this.FieldId);
            
         
            config.Setting = this.Setting;
            config.MediaUrlOptions = MediaUrlOptions;
           


            //code first configuration
            var fieldFieldValues = propertyInfo.GetCustomAttributes(typeof(SitecoreFieldFieldValueAttribute), true).Cast<SitecoreFieldFieldValueAttribute>();
 
            ////fix: fieldfieldvalues are not passed
            var interfaceFromProperty = propertyInfo.DeclaringType.GetInterfaces().FirstOrDefault(inter => inter.GetProperty(propertyInfo.Name) != null);
            if (interfaceFromProperty != null)
            {
                fieldFieldValues = interfaceFromProperty.GetProperty(propertyInfo.Name).GetCustomAttributes(typeof(SitecoreFieldFieldValueAttribute), true).Cast<SitecoreFieldFieldValueAttribute>(); ;
            }
                 
            base.Configure(propertyInfo, config);
        }
    }
}




