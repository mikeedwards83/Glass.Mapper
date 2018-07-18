using Glass.Mapper.Configuration;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration
{
    /// <summary>
    /// Class SitecoreChildrenConfiguration
    /// </summary>
    public class SitecoreChildrenConfiguration : ChildrenConfiguration
    {
        public ID TemplateId { get; set; }

        public SitecoreEnforceTemplate EnforceTemplate { get; set; }

        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new SitecoreChildrenConfiguration();
        }

        protected override void Copy(AbstractPropertyConfiguration copy)
        {
            var config = copy as SitecoreChildrenConfiguration;
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
            var locals = propertyOptions as GetItemsOptions;
            if (locals != null)
            {
                locals.EnforceTemplate = EnforceTemplate;
                locals.TemplateId = TemplateId;
            }

            base.GetPropertyOptions(propertyOptions);
        }
    }
}




