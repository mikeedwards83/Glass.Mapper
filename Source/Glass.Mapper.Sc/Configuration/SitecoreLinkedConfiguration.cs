using Glass.Mapper.Configuration;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration
{
    /// <summary>
    /// Class SitecoreLinkedConfiguration
    /// </summary>
    public class SitecoreLinkedConfiguration : LinkedConfiguration
    {
        /// <summary>
        /// Indicate weather All, References or Referred should be loaded
        /// </summary>
        /// <value>The option.</value>
        public SitecoreLinkedOptions Option { get; set; }

        public ID TemplateId { get; set; }
        public SitecoreEnforceTemplate EnforceTemplate { get; set; }

        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new SitecoreLinkedConfiguration();
        }

        protected override void Copy(AbstractPropertyConfiguration copy)
        {
            var config = copy as SitecoreLinkedConfiguration;
            config.Option = Option;
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
    }
}




