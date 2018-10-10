using System;
using System.Web.UI;
using Glass.Mapper.Sc.Builders;

namespace Glass.Mapper.Sc.Web.WebForms
{
    public static class IWebFormsContextExtensionMethods
    {
        public static T GetDataSourceItem<T>(this IWebFormsContext context, Control control, Action<GetKnownItemBuilder> config) where T:class
        {
            var builder=  new GetKnownItemBuilder();
            config(builder);
            return context.GetDataSourceItem<T>(control, builder);
        }

      

    }


}
