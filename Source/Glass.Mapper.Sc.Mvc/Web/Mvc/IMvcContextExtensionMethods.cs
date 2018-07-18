using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Builders;

namespace Glass.Mapper.Sc.Web.Mvc
{
    public static class IMvcContextExtensionMethods
    {
        public static T GetDataSourceItem<T>(this IMvcContext context, Action<GetKnownItemBuilder> config) where T:class
        {
            var builder=  new GetKnownItemBuilder();
            config(builder);
            return context.GetDataSourceItem<T>(builder);
        }

        public static T GetPageContextItem<T>(this IMvcContext context, Action<GetKnownItemBuilder> config) where T : class
        {
            var builder = new GetKnownItemBuilder();
            config(builder);
            return context.GetPageContextItem<T>(builder);
        }

        public static T GetRenderingItem<T>(this IMvcContext context, Action<GetKnownItemBuilder> config) where T : class
        {
            var builder = new GetKnownItemBuilder();
            config(builder);
            return context.GetRenderingItem<T>(builder);
        }

    }


}
