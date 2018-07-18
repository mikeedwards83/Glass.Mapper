using System.Reflection;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    /// <summary>
    /// Class SitecoreNodeAttribute
    /// </summary>
    public class SitecoreNodeAttribute : NodeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreNodeAttribute"/> class.
        /// </summary>
        public SitecoreNodeAttribute():base()
        {
        }


        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns>AbstractPropertyConfiguration.</returns>
        public override Mapper.Configuration.AbstractPropertyConfiguration Configure(System.Reflection.PropertyInfo propertyInfo)
        {
            var config = new SitecoreNodeConfiguration();
            Configure(propertyInfo, config);
            return config;
        }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="config">The config.</param>
        public void Configure(PropertyInfo propertyInfo, SitecoreNodeConfiguration config)
        {
            base.Configure(propertyInfo, config);
        }
    }
}




