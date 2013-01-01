using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using umbraco.interfaces;

namespace Glass.Mapper.Umb
{
    public class UmbracoTypeSavingContext : AbstractTypeSavingContext
    {
        public INode Node { get; set; }
    }
}
