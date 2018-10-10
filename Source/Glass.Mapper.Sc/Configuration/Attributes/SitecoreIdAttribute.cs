using System;
using System.Reflection;
using Glass.Mapper.Configuration.Attributes;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    /// <summary>
    /// Class SitecoreIdAttribute
    /// </summary>
    public class SitecoreIdAttribute : IdAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreIdAttribute"/> class.
        /// </summary>
        public SitecoreIdAttribute():base(new []{typeof(ID), typeof(Guid)} ) { }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns>AbstractPropertyConfiguration.</returns>
        public override Mapper.Configuration.AbstractPropertyConfiguration Configure(System.Reflection.PropertyInfo propertyInfo)
        {
            var config = new SitecoreIdConfiguration();
            base.Configure(propertyInfo, config);
            return config;
        }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="config">The config.</param>
        public void Configure(PropertyInfo propertyInfo, SitecoreIdConfiguration config)
        {
            base.Configure(propertyInfo, config);
        }
    }
}




