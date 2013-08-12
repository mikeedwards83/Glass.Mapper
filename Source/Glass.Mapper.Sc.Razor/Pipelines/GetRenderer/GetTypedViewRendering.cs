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
using Sitecore.Mvc.Pipelines.Response.GetRenderer;
using Glass.Mapper.Sc.Razor.Web.Mvc;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Razor.Pipelines.GetRenderer
{
    /// <summary>
    /// Class GetTypedViewRendering
    /// </summary>
    public class GetTypedViewRendering : AbstractGetViewRendering
    {
        /// <summary>
        /// Gets the renderer.
        /// </summary>
        /// <param name="rendering">The rendering.</param>
        /// <param name="args">The args.</param>
        /// <returns>Sitecore.Mvc.Presentation.Renderer.</returns>
        protected override Sitecore.Mvc.Presentation.Renderer GetRenderer(
            Sitecore.Mvc.Presentation.Rendering rendering, 
            GetRendererArgs args)
        {

            var renderItem = rendering.Item.Database.GetItem(new ID(rendering.RenderingItemPath));
            if (renderItem.TemplateName == "GlassTypedRazor")
            {
                TypedViewRenderer render = new TypedViewRenderer();
                render.Path = renderItem["Name"];
                render.ContextName = renderItem["ContextName"];

                render.DataSource = rendering.DataSource;
                return render;
            }

            return null;
        }
    }
}

