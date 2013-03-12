﻿using System;
using System.Linq;
using System.Collections.Specialized;
using Sitecore.Web.UI;
using System.Web.UI;
using Glass.Mapper.Sc.Razor.Web.Ui;

namespace Glass.Mapper.Sc.Razor.RenderingTypes
{
    public class DynamicRazorRenderingType : RenderingType
    {
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
        public static System.Web.UI.WebControls.WebControl CreateControl(string view, string contextName)
        {
            IRazorControl control = global::Sitecore.Reflection.ReflectionUtil.CreateObject(typeof(DynamicControl)) as IRazorControl;
            control.View = view;
            control.ContextName = contextName;
            return control as System.Web.UI.WebControls.WebControl;
        }
    }

}
