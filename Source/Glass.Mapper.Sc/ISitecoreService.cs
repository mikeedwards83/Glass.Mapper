using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    public interface ISitecoreService: IAbstractService
    {
        Database Database { get; }
        object CreateClass(Type type, Item item, bool isLazy = false, bool inferType = false);

    }
}
