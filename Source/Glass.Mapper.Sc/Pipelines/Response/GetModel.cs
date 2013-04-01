using System;
using Sitecore.Data.Items;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Pipelines.Response.GetModel;
using Sitecore.Mvc.Presentation;

namespace Glass.Mapper.Sc.Pipelines.Response
{
    public class GetModel : GetModelProcessor
    {
        public GetModel()
        {
            ContextName = "Default";

        }
        public string ContextName { get; set; }

        public override void Process(GetModelArgs args)
        {
            if (args.Result == null)
            {

                Type type = GetFromField(args.Rendering, args);

                if (type == null)
                    return;

                var context = Context.Contexts[ContextName];
                if (context == null) throw new MapperException("Failed to find context {0}".Formatted(ContextName));

                if (context.TypeConfigurations.ContainsKey(type))
                {
                    ISitecoreContext scContext = new SitecoreContext(context);
                    var result = scContext.GetCurrentItem(type);
                    args.Result = result;
                }
            }
        }

        protected virtual Type GetFromField(Rendering rendering, GetModelArgs args)
        {

            Item obj = ObjectExtensions.ValueOrDefault<RenderingItem, Item>(rendering.RenderingItem, (Func<RenderingItem, Item>)(i => i.InnerItem));
            if (obj == null)
                return null;
            else
                return Type.GetType(obj["Model"], true);
        }
    }
}
