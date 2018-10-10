using Glass.Mapper.Configuration;
using Sitecore.Diagnostics;

namespace Glass.Mapper.Sc.Configuration
{
    /// <summary>
    /// Class SitecoreNodeConfiguration
    /// </summary>
    public class SitecoreNodeConfiguration : NodeConfiguration
    {
        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new SitecoreNodeConfiguration();
        }

        protected override void Copy(AbstractPropertyConfiguration copy)
        {
            var config = copy as SitecoreNodeConfiguration;

            base.Copy(copy);
        }

        public virtual void GetPropertyOptions(GetOptions propertyOptions)
        {
            var scPropertyOptions = propertyOptions as GetItemOptions;

            Assert.IsNotNull(scPropertyOptions, "propertyOptions  must be of type GetItemOptions");

            scPropertyOptions.EnforceTemplate = SitecoreEnforceTemplate.Default;

            base.GetPropertyOptions(propertyOptions);
        }

       
    }
}




