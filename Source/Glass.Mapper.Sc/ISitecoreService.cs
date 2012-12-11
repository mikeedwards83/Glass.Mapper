using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    public interface ISitecoreService
    {
        object CreateClass(Type type, Item item, bool isLazy = false, bool inferType = false);

    }
}
