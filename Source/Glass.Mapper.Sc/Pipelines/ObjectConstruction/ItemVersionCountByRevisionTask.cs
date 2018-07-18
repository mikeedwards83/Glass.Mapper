using Glass.Mapper.Pipelines.ObjectConstruction;
using Sitecore;

namespace Glass.Mapper.Sc.Pipelines.ObjectConstruction
{
    public class ItemVersionCountByRevisionTask : AbstractObjectConstructionTask
    {
        public override void Execute(ObjectConstructionArgs args)
        {
            var scContext = args.AbstractTypeCreationContext as SitecoreTypeCreationContext;
            var options = args.Options as GetItemOptions;

            if (scContext != null 
                && options != null
                && options.VersionCount 
                && scContext.Item != null
                && scContext.Item[FieldIDs.Revision].IsNullOrEmpty())
            {

                args.Result = null;
            }
            else
            {
                base.Next(args);
            }
        }
    }
}
