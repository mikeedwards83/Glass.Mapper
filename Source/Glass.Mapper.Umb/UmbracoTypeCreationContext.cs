using Glass.Mapper.Umb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using umbraco.interfaces;

namespace Glass.Mapper.Umb
{
    public class UmbracoTypeCreationContext : AbstractTypeCreationContext
    {
     
        public INode Node { get; set; }

        public UmbracoService UmbracoService { get; set; }
    }
    
}
