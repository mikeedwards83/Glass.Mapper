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
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using Glass.Mapper.Sc.Web.Ui;
using Sitecore.Data;
using Sitecore.Web.UI.WebControls;

namespace Glass.Mapper.Sc.Web.Mvc
{
    /// <summary>
    /// HtmlHelperExtensions
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Begins the edit frame.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <returns></returns>
        public static GlassEditFrame BeginEditFrame(this HtmlHelper htmlHelper)
        {
            return BeginEditFrame(htmlHelper, "");
        }

        /// <summary>
        /// Begins the edit frame.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="dataSource">The data source.</param>
        /// <returns></returns>
        public static GlassEditFrame BeginEditFrame(this HtmlHelper htmlHelper, ID dataSource)
        {
            return BeginEditFrame(htmlHelper, dataSource.ToString());
        }

        /// <summary>
        /// Begins the edit frame.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="dataSource">The data source.</param>
        /// <returns></returns>
        public static GlassEditFrame BeginEditFrame(this HtmlHelper htmlHelper, string dataSource)
        {
            return BeginEditFrame(htmlHelper, dataSource, GlassEditFrame.DefaultEditButtons);
        }

        /// <summary>
        /// Begins the edit frame.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="dataSource">The data source.</param>
        /// <param name="buttons">The buttons.</param>
        /// <returns></returns>
        public static GlassEditFrame BeginEditFrame(this HtmlHelper htmlHelper, ID dataSource, ID buttons)
        {
            return BeginEditFrame(htmlHelper, dataSource.ToString(), buttons.ToString());
        }

        /// <summary>
        /// Begins the edit frame.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="dataSource">The data source.</param>
        /// <param name="buttons">The buttons.</param>
        /// <param name="title">The title for the edit frame</param>
        /// <returns></returns>
        public static GlassEditFrame BeginEditFrame(this HtmlHelper htmlHelper, ID dataSource, ID buttons, string title)
        {
            return BeginEditFrame(htmlHelper, dataSource.ToString(), buttons.ToString(), title);
        }

        /// <summary>
        /// Begins the edit frame.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="dataSource">The data source.</param>
        /// <param name="buttons">The buttons.</param>
        /// <returns></returns>
        public static GlassEditFrame BeginEditFrame(this HtmlHelper htmlHelper, ID dataSource, string buttons)
        {
            return BeginEditFrame(htmlHelper, dataSource.ToString(), buttons);
        }

        /// <summary>
        /// Begins the edit frame.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="dataSource">The data source.</param>
        /// <param name="buttons">The buttons.</param>
        /// <param name="title">The title for the edit frame</param>
        /// <returns></returns>
        public static GlassEditFrame BeginEditFrame(this HtmlHelper htmlHelper, ID dataSource, string buttons, string title)
        {
            return BeginEditFrame(htmlHelper, dataSource.ToString(), buttons, title);
        }

        /// <summary>
        /// Begins the edit frame.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="dataSource">The data source.</param>
        /// <param name="buttons">The buttons.</param>
        /// <returns></returns>
        public static GlassEditFrame BeginEditFrame(this HtmlHelper htmlHelper, string dataSource, ID buttons)
        {
            return BeginEditFrame(htmlHelper, dataSource, buttons.ToString());
        }

        /// <summary>
        /// Begins the edit frame.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="dataSource">The data source.</param>
        /// <param name="buttons">The buttons.</param>
        /// <param name="title">The title for the edit frame</param>
        /// <returns></returns>
        public static GlassEditFrame BeginEditFrame(this HtmlHelper htmlHelper, string dataSource, ID buttons, string title)
        {
            return BeginEditFrame(htmlHelper, dataSource, buttons.ToString(), title);
        }



        /// <summary>
        /// Begins the edit frame.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="dataSource">The data source.</param>
        /// <param name="buttons">The buttons.</param>
        /// <returns></returns>
        public static GlassEditFrame BeginEditFrame(this HtmlHelper htmlHelper, string dataSource, string buttons)
        {
            return BeginEditFrame(htmlHelper, dataSource, buttons.ToString(), string.Empty);

        }

        /// <summary>
        /// Begins the edit frame.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="dataSource">The data source.</param>
        /// <param name="buttons">The buttons.</param>
        /// <param name="title">The title for the edit frame</param>
        /// <returns></returns>
        public static GlassEditFrame BeginEditFrame(this HtmlHelper htmlHelper, string dataSource, string buttons, string title)
        {
            var frame = new GlassEditFrame(title, buttons, htmlHelper.ViewContext.Writer, dataSource);
            frame.RenderFirstPart();
            return frame;
        }

        /// <summary>
        /// Begins the edit frame.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="editFrame">The edit frame.</param>
        /// <returns></returns>
        public static GlassEditFrame BeginEditFrame(this HtmlHelper htmlHelper, EditFrame editFrame)
        {
            var writter = new HtmlTextWriter(htmlHelper.ViewContext.Writer);
            editFrame.RenderFirstPart(writter);
            return new GlassEditFrame(editFrame);
        }

        public static GlassHtmlMvc<T> Glass<T>(this HtmlHelper<T> htmlHelper)
        {
            var html = new GlassHtml(SitecoreContext.GetFromHttpContext());
            return new GlassHtmlMvc<T>(html, htmlHelper.ViewContext.Writer, htmlHelper.ViewData.Model);
        }
    }
}

