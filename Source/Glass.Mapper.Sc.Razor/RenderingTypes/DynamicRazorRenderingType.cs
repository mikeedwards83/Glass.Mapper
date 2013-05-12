﻿using System;
using System.Linq;
using System.Collections.Specialized;
using Sitecore.Web.UI;
using System.Web.UI;
using Glass.Mapper.Sc.Razor.Web.Ui;

namespace Glass.Mapper.Sc.Razor.RenderingTypes
{
    /// <summary>
    /// Class DynamicRazorRenderingType
    /// </summary>
    public class DynamicRazorRenderingType : RenderingType
    {
        /// <summary>
        /// Gets the control.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="assert">if set to <c>true</c> this instance performs asserts.</param>
        /// <returns>The created control.</returns>
        /// <exception cref="Glass.Mapper.Sc.Razor.RazorException">Parameters does not contain parameter 'Name'</exception>
        /// <exception cref="System.NullReferenceException">Parameter 'Name' is null or empty</exception>
        public override Control GetControl(NameValueCollection parameters, bool assert)
        {
            if(!parameters.AllKeys.Any(x=>x=="Name"))
                throw new RazorException("Parameters does not contain parameter 'Name'");
            
            
            string view = parameters["Name"];
            string contextName = parameters["ContextName"];
            if(view.IsNullOrEmpty())
                throw new NullReferenceException("Parameter 'Name' is null or empty");


            return CreateControl(view, contextName);
        }
        /// <summary>
        /// Creates the control.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="contextName">Name of the context.</param>
        /// <returns>System.Web.UI.WebControls.WebControl.</returns>
        public static System.Web.UI.WebControls.WebControl CreateControl(string view, string contextName)
        {
            IRazorControl control = global::Sitecore.Reflection.ReflectionUtil.CreateObject(typeof(DynamicControl)) as IRazorControl;
            control.View = ViewManager.GetRazorView(view); ;
            control.ContextName = contextName;
            return control as System.Web.UI.WebControls.WebControl;
        }
    }

}
