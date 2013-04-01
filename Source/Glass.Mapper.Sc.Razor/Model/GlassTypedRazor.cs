using Glass.Mapper.Sc.Configuration.Attributes;

namespace Glass.Mapper.Sc.Razor.Model
{
    /// <summary>
    /// Class GlassTypedRazor
    /// </summary>
    [SitecoreType(TemplateId = "{7B10C01D-B0DF-4626-BE34-F48E38828FB7}")]
    public class GlassTypedRazor
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
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        [SitecoreField]
        public string Code { get; set; }

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
