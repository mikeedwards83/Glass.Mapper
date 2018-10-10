using System;
using System.Web.UI;
using Glass.Mapper.Sc.Builders;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.Web.WebForms
{
    public interface IWebFormsContext : IRequestContext
    {
        IGlassHtml GlassHtml { get; set; }
        bool GetHasDataSource(Control control);
        Item GetDataSourceItem(Control control);
        string GetRenderingParameters(Control control);

        T GetDataSourceItem<T>(Control control) where T : class;
        T GetDataSourceItem<T>(Control control, GetKnownOptions options) where T : class;
    }
}