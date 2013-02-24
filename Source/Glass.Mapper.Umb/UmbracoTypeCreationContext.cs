using Glass.Mapper.Umb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Core.Models;

namespace Glass.Mapper.Umb
{
    public class UmbracoTypeCreationContext : AbstractTypeCreationContext
    {
     
        public IContent Content { get; set; }

        public UmbracoService UmbracoService { get; set; }
    }
    
}
