using System;
using System.IO;
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
        /// <param name="writer">The writer.</param>
        public GlassEditFrame(EditFrame frame, TextWriter writer)
        {
            _frame = frame;
            _writer = new HtmlTextWriter(writer);
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
        public GlassNullEditFrame() : base(null, null)
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




