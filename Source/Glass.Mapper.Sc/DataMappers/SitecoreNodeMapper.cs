using System;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// Class SitecoreNodeMapper
    /// </summary>
    public class SitecoreNodeMapper : AbstractDataMapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreNodeMapper"/> class.
        /// </summary>
        public SitecoreNodeMapper()
        {
            ReadOnly = true;
        }


        /// <summary>
        /// Maps data from the .Net property value to the CMS value
        /// </summary>
        /// <param name="mappingContext">The mapping context.</param>
        /// <returns>The value to write</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Maps data from the CMS value to the .Net property value
        /// </summary>
        /// <param name="mappingContext">The mapping context.</param>
        /// <returns>System.Object.</returns>
        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var scConfig = Configuration as SitecoreNodeConfiguration;
            var scContext = mappingContext as SitecoreDataMappingContext;
            var item = scContext.Item;

            Item targetItem = null;

            if (scConfig.Id.HasValue())
            {
                var guid = Guid.Empty;

                if (Guid.TryParse(scConfig.Id, out guid) && guid != Guid.Empty)
                {
                    targetItem = item.Database.GetItem(new ID(guid), item.Language);
                }
            }
            else if (!scConfig.Path.IsNullOrEmpty())
            {
                targetItem = item.Database.GetItem(scConfig.Path, item.Language);
            }

            var options = new GetItemByItemOptions();
            options.Copy(mappingContext.Options);
            options.Item = targetItem;

            scConfig.GetPropertyOptions(options);

            return scContext.Service.GetItem(options);

        }



        /// <summary>
        /// Indicates that the data mapper will mapper to and from the property
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if this instance can handle the specified configuration; otherwise, <c>false</c>.</returns>
        public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is SitecoreNodeConfiguration;
        }
    }
}




