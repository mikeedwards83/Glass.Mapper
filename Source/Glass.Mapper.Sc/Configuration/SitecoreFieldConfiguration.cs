using Glass.Mapper.Configuration;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration
{
    /// <summary>
    /// Class SitecoreFieldConfiguration
    /// </summary>
    public class SitecoreFieldConfiguration : FieldConfiguration
    {
        /// <summary>
        /// The name of the field  to use if it is different to the property name
        /// </summary>
        /// <value>The name of the field.</value>
        public string FieldName { get; set; }

        /// <summary>
        /// The ID of the field when used in a code first scenario
        /// </summary>
        /// <value>The field id.</value>
        public ID FieldId { get; set; }

        /// <summary>
        /// Options to override the behaviour of certain fields.
        /// </summary>
        /// <value>The setting.</value>
        public SitecoreFieldSettings Setting { get; set; }

        /// <summary>
        /// Use with Glass.Mapper.Sc.Fields.Link type
        /// </summary>
        public SitecoreInfoUrlOptions UrlOptions { get; set; }

        public SitecoreMediaUrlOptions MediaUrlOptions { get; set; }

        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new SitecoreFieldConfiguration();
        }

        protected override void Copy(AbstractPropertyConfiguration copy)
        {
            var config = copy as SitecoreFieldConfiguration;
            config.FieldId = FieldId;
            config.FieldName = FieldName;
            config.Setting = Setting;
            config.MediaUrlOptions = MediaUrlOptions;
            config.UrlOptions = UrlOptions;
            base.Copy(copy);
        }
      

        public virtual void GetPropertyOptions(GetOptions propertyOptions)
        {
            propertyOptions.InferType = (Setting & SitecoreFieldSettings.InferType) == SitecoreFieldSettings.InferType;
            base.GetPropertyOptions(propertyOptions);
        }
    }
}





