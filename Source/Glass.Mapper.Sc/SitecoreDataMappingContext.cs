using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.IoC;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    /// <summary>
    /// Class SitecoreDataMappingContext
    /// </summary>
    public class SitecoreDataMappingContext:AbstractDataMappingContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreDataMappingContext"/> class.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="item">The item.</param>
        /// <param name="service">The service.</param>
        public SitecoreDataMappingContext(object obj, Item item, ISitecoreService service, GetItemOptions getModelOptions)
            :base(obj, getModelOptions)
        {

            //TODO: ME - should we assert that these are not null
            this.Item = item;
            Service = service;
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <value>The item.</value>
        public Item Item { get; private set; }
        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <value>The service.</value>
        public ISitecoreService Service { get; private set; }
    }
}




