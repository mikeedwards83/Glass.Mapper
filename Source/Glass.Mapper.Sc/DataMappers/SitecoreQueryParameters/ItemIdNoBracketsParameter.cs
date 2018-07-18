
namespace Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters
{
    /// <summary>
    /// Class ItemIdNoBracketsParameter
    /// </summary>
    public class ItemIdNoBracketsParameter : ISitecoreQueryParameter
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
                return "id:N";
            }

        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.String.</returns>
        public string GetValue(global::Sitecore.Data.Items.Item item)
        {
            return item.ID.Guid.ToString("N");
        }

        #endregion
    }

}




