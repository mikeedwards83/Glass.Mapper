using System;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Sc.Configuration
{
    /// <summary>
    /// The sitecore delegate configuration
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SitecoreDelegateConfiguration : AbstractPropertyConfiguration
    {
        /// <summary>
        /// Gets or sets the action to take place when mapping to the cms
        /// </summary>
        public Action<SitecoreDataMappingContext> MapToCmsAction { get; set; }

        /// <summary>
        /// Gets or sets the action to take place when mapping to the objects property
        /// </summary>
        public Func<SitecoreDataMappingContext, object> MapToPropertyAction { get; set; }

        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new SitecoreDelegateConfiguration();
        }

        protected override void Copy(AbstractPropertyConfiguration copy)
        {
            var config = copy as SitecoreDelegateConfiguration;

            config.MapToCmsAction = MapToCmsAction;
            config.MapToPropertyAction = MapToPropertyAction;

            base.Copy(copy);
        }
    }
}
