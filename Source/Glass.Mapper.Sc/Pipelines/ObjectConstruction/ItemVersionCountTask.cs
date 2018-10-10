using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.Pipelines.ObjectConstruction
{

    /// <summary>
    /// Performs a version count using the Item.Versions.Count
    /// </summary>
    public class ItemVersionCountTask : AbstractObjectConstructionTask
    {
        public override void Execute(ObjectConstructionArgs args)
        {
            var scContext = args.AbstractTypeCreationContext as SitecoreTypeCreationContext;
            var options = args.Options as GetItemOptions;

            if (scContext != null
                && options != null
                && options.VersionCount
                && scContext.Item.Versions.Count == 0)
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
