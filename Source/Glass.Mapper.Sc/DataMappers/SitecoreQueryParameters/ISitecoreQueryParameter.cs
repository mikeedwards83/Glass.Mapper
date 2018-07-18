using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters
{
    /// <summary>
    /// Interface ISitecoreQueryParameter
    /// </summary>
    public interface ISitecoreQueryParameter
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.String.</returns>
        string GetValue(Item item);
    }
}




