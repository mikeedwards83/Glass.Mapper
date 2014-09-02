using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data;

namespace Glass.Mapper.Sc.CodeFirst
{
    class GlassItemDefinition: ItemDefinition
    {
        public GlassItemDefinition(Sitecore.Data.ID itemId, string itemName, Sitecore.Data.ID templateId, Sitecore.Data.ID branchId): base(itemId, itemName, templateId, branchId)
        {
        }
    }
}
