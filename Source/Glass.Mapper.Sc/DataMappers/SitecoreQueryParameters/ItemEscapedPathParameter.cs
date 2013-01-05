using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters
{
    public class ItemEscapedPathParameter : ISitecoreQueryParameter
    {
        #region ISitecoreQueryParameter Members

        public string Name
        {
            get { return "escapedPath"; }
        }

        public string GetValue(global::Sitecore.Data.Items.Item item)
        {
            string path = item.Paths.FullPath;
            string[] pathSections = path.Split('/');

            StringBuilder escapedPath = new StringBuilder();

            foreach (var pathSection in pathSections)
            {
                if (!string.IsNullOrEmpty(pathSection)) escapedPath.AppendFormat("/#{0}#", pathSection);
            }

            return escapedPath.ToString();
        }

        #endregion
    }

}
