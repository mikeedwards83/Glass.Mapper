using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters
{
    public class ItemIdNoBracketsParameter : ISitecoreQueryParameter
    {
        #region ISitecoreQueryParameter Members

        public string Name
        {
            get
            {
                return "id:N";
            }

        }

        public string GetValue(global::Sitecore.Data.Items.Item item)
        {
            return item.ID.Guid.ToString("N");
        }

        #endregion
    }

}
