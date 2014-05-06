using System.Reflection;

namespace Glass.Mapper.Configuration.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ItemAttribute : AbstractPropertyAttribute
    {
        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="config">The config.</param>
        public void Configure(PropertyInfo propertyInfo, InfoConfiguration config)
        {
            base.Configure(propertyInfo, config);
        }
    }
}
