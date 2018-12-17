using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters
{
    /// <summary>
    /// Class ItemDateNowParameter
    /// </summary>
    public class ItemDateNowParameter : ISitecoreQueryParameter
    {

        #region ISitecoreQueryParameter Members

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return "dateNow"; }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.String.</returns>
        public string GetValue(global::Sitecore.Data.Items.Item item)
        {
            return global::Sitecore.DateUtil.ToIsoDate(DateTime.Now);
        }

        #endregion
    }
}




