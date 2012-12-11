using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    public class SitecoreDataMappingContext:AbstractDataMappingContext
    {
        public SitecoreDataMappingContext(object obj, Item item, ISitecoreService service):base(obj)
        {

            //TODO: ME - should we assert that these are not null
            this.Item = item;
            Service = service;
        }

        public Item Item { get; private set; }
        public ISitecoreService Service { get; private set; }
    }
}
