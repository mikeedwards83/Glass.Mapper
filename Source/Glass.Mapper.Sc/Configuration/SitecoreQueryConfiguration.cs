using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Configuration;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration
{
    /// <summary>
    /// Class SitecoreQueryConfiguration
    /// </summary>
    public class SitecoreQueryConfiguration : QueryConfiguration
    {
        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new SitecoreQueryConfiguration();
        }

        protected override void Copy(AbstractPropertyConfiguration copy)
        {
            var config = copy as SitecoreQueryConfiguration;
            config.TemplateId = TemplateId;
            config.EnforceTemplate = EnforceTemplate;
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

        public ID TemplateId { get; set; }
        public SitecoreEnforceTemplate EnforceTemplate { get; set; }
    }
}




