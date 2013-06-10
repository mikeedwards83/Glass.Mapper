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
using Sitecore.Web.UI;
using System.ComponentModel;
using Glass.Mapper.Sc.Razor.Web.Ui;

namespace Glass.Mapper.Sc.Razor
{
    /// <summary>
    /// Class ContextRazorView
    /// </summary>
    public class ContextRazorView : WebControl
    {

        /// <summary>
        /// Gets or sets the name of the type.
        /// </summary>
        /// <value>The name of the type.</value>
        public string TypeName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the assembly.
        /// </summary>
        /// <value>The name of the assembly.</value>
        public string AssemblyName { get; set; }

        /// <summary>
        /// Gets the <see cref="T:System.Web.HttpContext" /> object associated with the server control for the current Web request.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        /// <returns>The specified <see cref="T:System.Web.HttpContext" /> object associated with the current request.</returns>
        public string Context { get; set; }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {

            Type type =  Type.GetType(TypeName);

            Type razorControlType = typeof(AbstractRazorControl<>);
            Type finalControlType = razorControlType.MakeGenericType(type);

            WebControl finalControl =Activator.CreateInstance(finalControlType) as WebControl;



            ISitecoreContext _context = new SitecoreContext(Context);
            var model = _context.GetCurrentItem(type);

            TypeDescriptor.GetProperties(finalControlType).Find("Model", false).SetValue(finalControl, model);

            this.Controls.Add(finalControl);

            base.CreateChildControls();
        }

        /// <summary>
        /// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"></see> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="output">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the server control content.</param>
        /// <remarks>When developing custom server controls, you can override this method to generate content for an ASP.NET page.</remarks>
        protected override void DoRender(System.Web.UI.HtmlTextWriter output)
        {
           //nothing happens here :-)
        }
    }
}

