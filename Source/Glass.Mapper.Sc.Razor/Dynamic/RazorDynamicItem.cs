using Glass.Mapper.Sc.Dynamic;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.Razor.Dynamic
{
    /// <summary>
    /// Class RazorDynamicItem
    /// </summary>
    public class RazorDynamicItem : DynamicItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicItem" /> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public RazorDynamicItem(Item item):base(item)
        {
        }
        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="item">The item.</param>
        /// <returns>System.Object.</returns>
        protected override object GetField(string fieldName, global::Sitecore.Data.Items.Item item)
        {
            return new DynamicField(fieldName, item);
        }

        /// <summary>
        /// Creates the new.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>DynamicItem.</returns>
        protected override DynamicItem CreateNew(Item item)
        {
            return new RazorDynamicItem(item);
        }
    }
}
