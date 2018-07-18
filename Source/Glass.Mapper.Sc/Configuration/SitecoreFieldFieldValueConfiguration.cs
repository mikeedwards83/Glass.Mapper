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
        /// <value>The field id.</value>
        public ID FieldId { get; set; }

        /// <summary>
        /// The value for the field if using Code First
        /// </summary>
        /// <value>The field value.</value>
        public string FieldValue { get; set; }
    }
}

