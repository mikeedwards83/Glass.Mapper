using Glass.Mapper.Sc.Configuration.Attributes;

namespace Glass.Mapper.Sc.Upgrade.Configuration.Attribute
{
    /// <summary>
    /// SitecoreClassAttribute
    /// </summary>
    public class SitecoreClassAttribute : SitecoreTypeAttribute
    {
        /// <summary>
        /// Indicates that the class can be used by Glass Sitecore Mapper
        /// </summary>
        public SitecoreClassAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreClassAttribute"/> class.
        /// </summary>
        /// <param name="codeFirst">if set to <c>true</c> [code first].</param>
        /// <param name="templateId">The template id.</param>
        public SitecoreClassAttribute(bool codeFirst, string templateId):base(codeFirst, templateId)
        {
        }

       

       
    }
}
