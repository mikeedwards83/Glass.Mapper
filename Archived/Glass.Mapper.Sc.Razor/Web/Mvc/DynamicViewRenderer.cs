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
using Glass.Mapper.Sc.Razor.RenderingTypes;
using System.Web.UI;
using Glass.Mapper.Sc.Razor.Web.Ui;

namespace Glass.Mapper.Sc.Razor.Web.Mvc
{
    /// <summary>
    /// Class DynamicViewRenderer
    /// </summary>
    public class DynamicViewRenderer :AbstractViewRendering
    {

        /// <summary>
        /// Renders the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public override void Render(System.IO.TextWriter writer)
        {
            DynamicControl control = DynamicRazorRenderingType.CreateControl(Path, ContextName) as DynamicControl;
            if (control != null)
            {
                control.Parameters = Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull.Rendering[Sc.GlassHtml.Parameters] ?? string.Empty;

                HtmlTextWriter htmlWriter = new HtmlTextWriter(writer);

                control.DataSource = this.DataSource;

                control.RenderControl(htmlWriter);
            }

        }
        

      
    }
}

