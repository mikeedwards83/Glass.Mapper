using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;

namespace Glass.Mapper.Sc.Razor.Web.Ui
{
    /// <summary>
    /// ITemplateBase
    /// </summary>
    public interface  ITemplateBase
    {
        /// <summary>
        /// Configures the specified sitecore context.
        /// </summary>
        /// <param name="sitecoreContext">The sitecore context.</param>
        /// <param name="viewData">The view data.</param>
        /// <param name="parentControl">The parent control.</param>
        void Configure(ISitecoreContext sitecoreContext, ViewDataDictionary viewData, Control parentControl);
    }
}
