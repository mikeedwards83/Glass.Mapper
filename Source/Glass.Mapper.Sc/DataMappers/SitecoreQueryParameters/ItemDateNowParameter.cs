using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters
{
    public class ItemDateNowParameter : ISitecoreQueryParameter
    {

        #region ISitecoreQueryParameter Members

        public string Name
        {
            get { return "dataNow"; }
        }

        public string GetValue(global::Sitecore.Data.Items.Item item)
        {
            return global::Sitecore.DateUtil.ToIsoDate(DateTime.Now);
        }

        #endregion
    }
}
