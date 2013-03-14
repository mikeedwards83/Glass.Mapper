﻿using System;
using System.Collections.Specialized;
using System.Web.UI;

namespace Glass.Mapper.Sc.Razor.RenderingTypes
{
    /// <summary>
    /// Class BehindRazorRenderingType
    /// </summary>
    public class BehindRazorRenderingType : AbstractCachingRenderingType
    {
        static Func<string, Type> _typeLoader = typeName => Type.GetType(typeName);

        /// <summary>
        /// Gets the control.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="assert">if set to <c>true</c> this instance performs asserts.</param>
        /// <returns>The created control.</returns>
        public override Control GetControl(NameValueCollection parameters, bool assert)
        {
            string view = parameters["Name"];
            string type = parameters["Type"];
            string assembly = parameters["Assembly"];
            string contextName = parameters["ContextName"];

            return CreateControl(view, type, assembly, contextName);
        }

        /// <summary>
        /// Creates the control.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="type">The type.</param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="contextName">Name of the context.</param>
        /// <returns>Sitecore.Web.UI.WebControl.</returns>
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
