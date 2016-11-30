using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Sc.Pipelines.ObjectConstruction
{
    public class SitecoreItemTask : AbstractObjectConstructionTask
    {
        public SitecoreItemTask()
        {
            Name = "SitecoreItemTask";
        }

        public override void Execute(ObjectConstructionArgs args)
        {
            if (args.Result == null && args.Configuration == Sc.Pipelines.ConfigurationResolver.SitecoreItemResolverTask.Config)
            {
                var scArgs = args.AbstractTypeCreationContext as SitecoreTypeCreationContext;
                args.Result = scArgs.Item;
            }
            base.Execute(args);
        }
    }
}
