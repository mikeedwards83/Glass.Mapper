using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    /// <summary>
    /// Class SitecoreTypeSavingContext
    /// </summary>
    public class SitecoreTypeSavingContext : AbstractTypeSavingContext
    {
        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>The item.</value>
        public Item Item { get; set; }


        public override AbstractDataMappingContext CreateDataMappingContext(IAbstractService service)
        {

            var mappingContext = new SitecoreDataMappingContext(Object, Item, service as ISitecoreService, null);
            return mappingContext;
        }
    }
}




