using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Builders;

namespace Glass.Mapper.Sc.Web
{
    public static class IRequestContextExtensionMethods
    {
        public static T GetContextItem<T>(this IRequestContext context, Action<GetKnownItemBuilder> config) where T : class
        {
            var builder = new GetKnownItemBuilder();
            config(builder);
            return context.GetContextItem<T>(builder);
        }

        public static T GetHomeItem<T>(this IRequestContext context, Action<GetKnownItemBuilder> config) where T : class
        {
            var builder = new GetKnownItemBuilder();
            config(builder);
            return context.GetHomeItem<T>(builder);
        }

        public static T GetRootItem<T>(this IRequestContext context, Action<GetKnownItemBuilder> config) where T : class
        {
            var builder = new GetKnownItemBuilder();
            config(builder);
            return context.GetRootItem<T>(builder);
        }
    }
}
