using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using Glass.Mapper.Sc.Web.Ui;
using Sitecore.Data;
using Sitecore.Web.UI.WebControls;

namespace Glass.Mapper.Sc.Web.Mvc
{
    public static class HtmlHelperExtensions
    {
        public static GlassEditFrame BeginEditFrame(this HtmlHelper htmlHelper)
        {
            return BeginEditFrame(htmlHelper, "");
        }

        public static GlassEditFrame BeginEditFrame(this HtmlHelper htmlHelper, ID dataSource)
        {
            return BeginEditFrame(htmlHelper, dataSource.ToString());
        }

        public static GlassEditFrame BeginEditFrame(this HtmlHelper htmlHelper, string dataSource)
        {
            return BeginEditFrame(htmlHelper, dataSource, GlassEditFrame.DefaultEditButons);
        }

        public static GlassEditFrame BeginEditFrame(this HtmlHelper htmlHelper, ID dataSource, ID buttons)
        {
            return BeginEditFrame(htmlHelper, dataSource.ToString(), buttons.ToString());
        }

        public static GlassEditFrame BeginEditFrame(this HtmlHelper htmlHelper, ID dataSource, string buttons)
        {
            return BeginEditFrame(htmlHelper, dataSource.ToString(), buttons);
        }

        public static GlassEditFrame BeginEditFrame(this HtmlHelper htmlHelper, string dataSource, ID buttons)
        {
            return BeginEditFrame(htmlHelper, dataSource, buttons.ToString());
        }

        public static GlassEditFrame BeginEditFrame(this HtmlHelper htmlHelper, string dataSource, string buttons)
        {
            var frame = new GlassEditFrame(buttons, htmlHelper.ViewContext.Writer, dataSource);
            frame.RenderFirstPart();
            return frame;
        }

        public static GlassEditFrame BeginEditFrame(this HtmlHelper htmlHelper, EditFrame editFrame)
        {
            var writter = new HtmlTextWriter(htmlHelper.ViewContext.Writer);
            editFrame.RenderFirstPart(writter);
            return new GlassEditFrame(editFrame);
        }
    }
}
