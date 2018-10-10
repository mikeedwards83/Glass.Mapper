using Sitecore.Data;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    /// <summary>
    /// Class SitecoreTypeCreationContext
    /// </summary>
    public class SitecoreTypeCreationContext : AbstractTypeCreationContext
    {

        /// <summary>
        /// Gets or sets the templateid.
        /// </summary>
        public ID TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>The item.</value>
        public Item Item { get; set; }

        /// <summary>
        /// Gets or sets the sitecore service.
        /// </summary>
        /// <value>The sitecore service.</value>
        public ISitecoreService SitecoreService { get; set; }

        public override string DataSummary()
        {
            if (Item == null)
            {
                return "Item is null";
            }
            return Item.Paths.FullPath;
        }


        /// <summary>
        /// Creates the data mapping context.
        /// </summary>
        public override AbstractDataMappingContext CreateDataMappingContext(object obj)
        {

            var mappingContext = new SitecoreDataMappingContext(obj, Item, SitecoreService, Options as GetItemOptions);
            return mappingContext;
        }

        public override bool CacheEnabled
        {
            get { return SitecoreService.CacheEnabled 
                    && (Sitecore.Context.Site == null || Sitecore.Context.PageMode.IsNormal); }
        }

    }
}
    





