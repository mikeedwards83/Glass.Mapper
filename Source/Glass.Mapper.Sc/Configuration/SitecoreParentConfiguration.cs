using Glass.Mapper.Configuration;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration
{
    /// <summary>
    /// Class SitecoreParentConfiguration
    /// </summary>
    public class SitecoreParentConfiguration : ParentConfiguration
    {
        public SitecoreEnforceTemplate EnforceTemplate { get; set; }

        public ID TemplateId { get; set; }

        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new SitecoreParentConfiguration();
        }

        protected override void Copy(AbstractPropertyConfiguration copy)
        {
            var config = copy as SitecoreParentConfiguration;

            config.EnforceTemplate = EnforceTemplate;
            config.TemplateId = TemplateId;

            base.Copy(copy);
        }

        public virtual void GetPropertyOptions(GetOptions propertyOptions)
        {
            var local = propertyOptions as GetItemOptions;

            if (local != null)
            {
                local.EnforceTemplate = EnforceTemplate;
                local.TemplateId = TemplateId;
            }
            base.GetPropertyOptions(propertyOptions);
        }
    }
}




