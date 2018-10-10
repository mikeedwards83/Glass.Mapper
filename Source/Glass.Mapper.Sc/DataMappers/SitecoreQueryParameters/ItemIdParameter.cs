

namespace Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters
{
    /// <summary>
    /// Class ItemIdParameter
    /// </summary>
    public class ItemIdParameter : ISitecoreQueryParameter
    {
        #region ISitecoreQueryParameter Members

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return "id";
            }

        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.String.</returns>
        public string GetValue(global::Sitecore.Data.Items.Item item)
        {
            return item.ID.ToString();
        }

        #endregion
    }
}




