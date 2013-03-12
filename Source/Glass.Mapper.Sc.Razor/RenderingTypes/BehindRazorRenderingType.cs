using System;
using System.Collections.Specialized;
using System.Web.UI;

namespace Glass.Mapper.Sc.Razor.RenderingTypes
{
    public class BehindRazorRenderingType : AbstractCachingRenderingType
    {
        static Func<string, Type> _typeLoader = typeName => Type.GetType(typeName);

        public override Control GetControl(NameValueCollection parameters, bool assert)
        {
            string view = parameters["Name"];
            string type = parameters["Type"];
            string assembly = parameters["Assembly"];
            string contextName = parameters["ContextName"];

            return CreateControl(view, type, assembly, contextName);
        }

        public static global::Sitecore.Web.UI.WebControl CreateControl(string view, string type, string assembly, string contextName)
        {
            string typeName = "{0}, {1}".Formatted(type, assembly);

            Type codeBehindType = GetControlType(typeName, _typeLoader);

            IRazorControl control = global::Sitecore.Reflection.ReflectionUtil.CreateObject(codeBehindType) as IRazorControl;
            control.View = view;
            control.ContextName = contextName;
            return control as global::Sitecore.Web.UI.WebControl;
        }
    
    }

}
