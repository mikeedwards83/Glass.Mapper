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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using Sitecore.Web.UI.WebControls;

namespace Glass.Mapper.Sc.Web.Ui
{

    /// <summary>
    /// Class GlassEditFrame
    /// </summary>
    public class GlassEditFrame : IDisposable
    {
        /// <summary>
        /// The default edit buttons
        /// </summary>
        public const string DefaultEditButtons = "/sitecore/content/Applications/WebEdit/Edit Frame Buttons/Default";

        EditFrame _frame;
        HtmlTextWriter _writer;
        /// <summary>
        /// Initializes a new instance of the <see cref="GlassEditFrame" /> class.
        /// </summary>
        /// <param name="buttons">The buttons.</param>
        /// <param name="writer">The writer.</param>
        /// <param name="dataSource">The data source.</param>
        public GlassEditFrame(string title, string buttons, TextWriter writer, string dataSource = "")
        {
            _frame = new EditFrame();
            _frame.DataSource = dataSource;
            _frame.Buttons = buttons;
            _frame.Title = title;
            _writer = new HtmlTextWriter(writer);

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlassEditFrame"/> class.
        /// </summary>
        /// <param name="frame">The frame.</param>
        public GlassEditFrame(EditFrame frame)
        {
            _frame = frame;
        }

        /// <summary>
        /// Renders the first part.
        /// </summary>
        public virtual void RenderFirstPart()
        {
            _frame.RenderFirstPart(_writer);
        }



        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            _frame.RenderLastPart(_writer);
        }
    }

    public class GlassNullEditFrame : GlassEditFrame
    {
        public GlassNullEditFrame() : base(null)
        {
        }

        public override void RenderFirstPart()
        {
            //DO NOTHING
        }

        public override void Dispose()
        {
            //DO NOTHING
        }
    }
}




