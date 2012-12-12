using Glass.Mapper.Configuration;

namespace Glass.Mapper.Umb.Configuration
{
    public class UmbracoFieldConfiguration : FieldConfiguration
    {
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

        /// <summary>
        /// The alias for the field if using Code First
        /// </summary>
        public string FieldAlias { get; set; }

        #endregion
    }
}
