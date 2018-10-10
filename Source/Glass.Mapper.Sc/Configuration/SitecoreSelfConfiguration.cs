using Glass.Mapper.Configuration;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration
{
    public class SitecoreSelfConfiguration : SelfConfiguration
    {
        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new SitecoreSelfConfiguration();
        }

        protected override void Copy(AbstractPropertyConfiguration copy)
        {
            var config = copy as SitecoreSelfConfiguration;
            config.TemplateId = TemplateId;
            config.EnforceTemplate = EnforceTemplate;
            base.Copy(copy);
        }

        public override void GetPropertyOptions(GetOptions propertyOptions)

        {
            var local = propertyOptions as GetItemOptions;

            if (local != null)
            {
                local.EnforceTemplate = EnforceTemplate;
                local.TemplateId = TemplateId;
            }

            base.GetPropertyOptions(propertyOptions);
        }

        public ID TemplateId { get; set; }
        public SitecoreEnforceTemplate EnforceTemplate { get; set; }
    }
}
