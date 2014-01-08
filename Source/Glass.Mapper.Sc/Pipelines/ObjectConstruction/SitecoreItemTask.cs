using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Sc.Pipelines.ObjectConstruction
{
    public class SitecoreItemTask : IObjectConstructionTask
    {
        public void Execute(ObjectConstructionArgs args)
        {
            if (args.Result == null && args.Configuration == Sc.Pipelines.ConfigurationResolver.SitecoreItemResolverTask.Config)
            {
                var scArgs = args.AbstractTypeCreationContext as SitecoreTypeCreationContext;
                args.Result = scArgs.Item;
            }
        }
    }
}
