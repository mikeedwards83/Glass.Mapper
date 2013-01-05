using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters
{
    public interface ISitecoreQueryParameter
    {
        string Name { get; }
        string GetValue(Item item);
    }
}
