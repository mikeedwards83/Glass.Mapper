
using System.Text;

namespace Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters
{
    /// <summary>
    /// Class ItemEscapedPathParameter
    /// </summary>
    public class ItemEscapedPathParameter : ISitecoreQueryParameter
    {
        #region ISitecoreQueryParameter Members

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return "escapedPath"; }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.String.</returns>
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




