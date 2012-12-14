using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration
{
    public class SitecoreFieldConfiguration : FieldConfiguration
    {
        /// <summary>
        /// The name of the field  to use if it is different to the property name
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// The ID of the field when used in a code first scenario 
        /// </summary>
        public ID FieldId { get; set; }

        /// <summary>
        /// Options to override the behaviour of certain fields.
        /// </summary>
        public SitecoreFieldSettings Setting { get; set; }

        #region Code First Properties

        /// <summary>
        /// Indicates the field should be used as part of a code first template
        /// </summary>
        public bool CodeFirst { get; set; }

        /// <summary>
        /// The type of field to create when using Code First
        /// </summary>
        public SitecoreFieldType FieldType { get; set; }

        /// <summary>
        /// The name of the section this field will appear in when using code first.
        /// </summary>
        public string SectionName { get; set; }

        /// <summary>
        /// The title for the field if using Code First
        /// </summary>
        public string FieldTitle { get; set; }

        /// <summary>
        /// The source for the field if using Code First
        /// </summary>
        public string FieldSource { get; set; }

        /// <summary>
        /// Sets the field as shared if using Code First
        /// </summary>
        public bool IsShared { get; set; }

        /// <summary>
        /// Sets the field as unversioned if using Code First
        /// </summary>
        public bool IsUnversioned { get; set; }

        #endregion
    }
}
