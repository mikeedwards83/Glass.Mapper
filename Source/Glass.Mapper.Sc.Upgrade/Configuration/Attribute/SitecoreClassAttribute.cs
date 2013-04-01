using Glass.Mapper.Sc.Configuration.Attributes;

namespace Glass.Mapper.Sc.Upgrade.Configuration.Attribute
{
    public class SitecoreClassAttribute : SitecoreTypeAttribute
    {
        /// <summary>
        /// Indicates that the class can be used by Glass Sitecore Mapper
        /// </summary>
        public SitecoreClassAttribute()
        {
        }

        public SitecoreClassAttribute(bool codeFirst, string templateId):base(codeFirst, templateId)
        {
        }

       

       
    }
}
