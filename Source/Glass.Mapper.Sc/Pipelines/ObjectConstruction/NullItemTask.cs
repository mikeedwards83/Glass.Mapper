using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Sc.Pipelines.ObjectConstruction
{
    public class NullItemTask : AbstractObjectConstructionTask
    {
        public override void Execute(ObjectConstructionArgs args)
        {
            var scContext = args.AbstractTypeCreationContext as SitecoreTypeCreationContext;

            if (scContext.Item != null)
            {
                base.Next(args);
            }
        }
    }
}
