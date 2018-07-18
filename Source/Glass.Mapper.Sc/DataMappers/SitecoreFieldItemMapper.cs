using System;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// Class SitecoreFieldItemMapper
    /// </summary>
    public class SitecoreFieldItemMapper : AbstractSitecoreFieldMapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreFieldItemMapper"/> class.
        /// </summary>
        public SitecoreFieldItemMapper()
            : base(typeof(Item))
        {
        }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.Object.</returns>
        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            Guid guid;
            return Guid.TryParse(fieldValue, out guid) ? context.Service.Database.GetItem(new ID(guid)) : null;
        }

        /// <summary>
        /// Sets the field value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="Glass.Mapper.MapperException">The value is not of type Sitecore.Data.Items.Item</exception>
        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            if (value == null)
                return null;

            var item = value as Item;
            if (item == null)
                throw new MapperException("The value is not of type Sitecore.Data.Items.Item");

            return item.ID.ToString();
        }
    }
}
