 using System;
using System.Collections.Specialized;
using System.Web.UI;
using Glass.Mapper.Sc.Razor.Web.Ui;

namespace Glass.Mapper.Sc.Razor.RenderingTypes
{
    /// <summary>
    /// Class TypedRazorRenderingType
    /// </summary>
    public class TypedRazorRenderingType : AbstractCachingRenderingType
    {
        static Func<string, Type> _typeLoader = typeName =>
                                                    {
                                                        return typeof (TypedControl);
                                                    };

        /// <summary>
        /// Gets the control.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="assert">if set to <c>true</c> this instance performs asserts.</param>
        /// <returns>The created control.</returns>
        public override Control GetControl(NameValueCollection parameters, bool assert)
        {
            string view = parameters["Name"];
            string contextName = parameters["ContextName"];

            return CreateControl(view, contextName);
        }

        /// <summary>
        /// Creates the control.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="contextName">Name of the context.</param>
        /// <returns>
        /// Sitecore.Web.UI.WebControl.
        /// </returns>
        public static global::Sitecore.Web.UI.WebControl CreateControl(string view, string contextName)
        {
            IRazorControl control = new TypedControl();
            control.View = ViewManager.GetRazorView(view); ;
            control.ContextName = contextName;
            return control as global::Sitecore.Web.UI.WebControl;
        }
    }

}
