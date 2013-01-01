using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters
{
    public class ItemPathParameter : ISitecoreQueryParameter
    {
        #region ISitecoreQueryParameter Members

        public string Name
        {
            get { return "path"; }
        }

        public string GetValue(global::Sitecore.Data.Items.Item item)
        {
            return item.Paths.FullPath;
        }

        #endregion
    }
}
