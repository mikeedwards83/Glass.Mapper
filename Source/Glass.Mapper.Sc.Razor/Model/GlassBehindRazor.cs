using Glass.Mapper.Sc.Configuration.Attributes;

namespace Glass.Mapper.Sc.Razor.Model
{
    /// <summary>
    /// Class GlassBehindRazor
    /// </summary>
    [SitecoreType(TemplateId="{9B162562-C999-45BF-B688-1C65D0EBCAAD}")]
    public class GlassBehindRazor
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

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [SitecoreField]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the assembly.
        /// </summary>
        /// <value>The assembly.</value>
        [SitecoreField]
        public string Assembly { get; set; }
    }
}
