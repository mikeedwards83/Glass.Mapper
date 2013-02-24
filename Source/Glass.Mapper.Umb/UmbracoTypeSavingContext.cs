using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Core.Models;

namespace Glass.Mapper.Umb
{
    public class UmbracoTypeSavingContext : AbstractTypeSavingContext
    {
        public IContent Content { get; set; }
    }
}
