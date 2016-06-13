/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-
using System;
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

            var razorView = ViewManager.GetRazorView(view);

            if (razorView == null)
            {
                return AbstractCachingRenderingType.ErrorControl(view);
            }

            IRazorControl control = Sitecore.Reflection.ReflectionUtil.CreateObject(typeof(DynamicControl)) as IRazorControl;
            control.View = razorView;
            control.ContextName = contextName;
            return control as System.Web.UI.WebControls.WebControl;
        }
    }

}

