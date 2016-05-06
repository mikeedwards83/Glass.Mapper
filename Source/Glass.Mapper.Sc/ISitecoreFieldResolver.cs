using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    public interface ISitecoreFieldResolver
    {
        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>Field.</returns>
        Field GetField(Item item, string fieldName);

        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="fieldId">Id of the field.</param>
        /// <returns>Field.</returns>
        Field GetField(Item item, ID fieldId);

        Field GetSitecoreField(Item item, SitecoreFieldConfiguration scConfig);
    }
}
