using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration
{
    /// <summary>
    /// Used to populate default values of a field's field
    /// </summary>
    public class SitecoreFieldFieldValueConfiguration
    {
        /// <summary>
        /// The Id (Guid) of the field to load
        /// </summary>
        public ID FieldId { get; set; }

        /// <summary>
        /// The value for the field if using Code First
        /// </summary>
        public string FieldValue { get; set; }
    }
}
