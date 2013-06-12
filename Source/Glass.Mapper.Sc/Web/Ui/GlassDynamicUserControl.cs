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
using Glass.Mapper.Sc.Dynamic;

namespace Glass.Mapper.Sc.Web.Ui
{
    /// <summary>
    /// Class GlassDynamicUserControl
    /// </summary>
    public class GlassDynamicUserControl : AbstractGlassUserControl
    {
        /// <summary>
        /// Model to render on the sublayout
        /// </summary>
        /// <value>The model.</value>
        public dynamic Model { get; private set; }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            Model = new DynamicItem(LayoutItem);
            base.OnLoad(e);
        }
    }
}

