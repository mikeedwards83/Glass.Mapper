using System.Reflection;
using Glass.Mapper.Configuration;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    /// <summary>
    /// SitecoreIgnoreAttribute
    /// </summary>
    public class SitecoreIgnoreAttribute : IgnoreAttribute
    {
        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns>
        /// AbstractPropertyConfiguration.
        /// </returns>
        public override AbstractPropertyConfiguration Configure(PropertyInfo propertyInfo)
        {
            var config = new SitecoreIgnoreConfiguration();
            base.Configure(propertyInfo, config);
            return config;
        }
    }
}

