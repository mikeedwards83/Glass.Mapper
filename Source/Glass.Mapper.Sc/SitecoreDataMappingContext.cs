using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    public class SitecoreDataMappingContext:AbstractDataMappingContext
    {
        public SitecoreDataMappingContext(object obj, Item item):base(obj)
        {
            this.Item = item;
        }

        public Item Item { get; private set; }
    }
}
