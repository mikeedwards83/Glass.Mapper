using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Sitecore;

namespace Glass.Mapper.Sc.Pipelines.ObjectConstruction
{
    public class ItemVersionCountAlwaysTrueTask : AbstractObjectConstructionTask
    {
        public override void Execute(ObjectConstructionArgs args)
        {
            base.Next(args);
        }
    }
}
