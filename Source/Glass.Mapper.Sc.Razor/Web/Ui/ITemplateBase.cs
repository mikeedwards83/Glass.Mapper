using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;

namespace Glass.Mapper.Sc.Razor.Web.Ui
{
    public interface  ITemplateBase
    {
        void Configure(ISitecoreContext sitecoreContext, ViewDataDictionary viewData, Control parentControl);
    }
}
