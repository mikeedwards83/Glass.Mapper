using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc
{
    public class SitecoreTypeCreationContext : AbstractTypeCreationContext
    {
        public ISitecoreService SitecoreService { get; set; }
     
        public Item Item { get; set; }
    }
    
}
