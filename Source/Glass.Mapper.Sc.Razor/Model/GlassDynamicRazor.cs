using Glass.Mapper.Sc.Configuration.Attributes;

namespace Glass.Mapper.Sc.Razor.Model
{
    /// <summary>
    /// Class GlassDynamicRazor
    /// </summary>
    [SitecoreType(TemplateId="{4432051D-8D3E-48E9-8C06-F1970EE607C5}")]
    public class GlassDynamicRazor
    {

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [SitecoreInfo(Configuration.SitecoreInfoType.Name)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>The view.</value>
        [SitecoreField("Name")]
        public string View { get; set; }
    }
}
