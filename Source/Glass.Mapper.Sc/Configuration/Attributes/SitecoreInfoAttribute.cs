
using System.Reflection;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    /// <summary>
    /// Class SitecoreInfoAttribute
    /// </summary>
    public class SitecoreInfoAttribute : InfoAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreInfoAttribute"/> class.
        /// </summary>
        public SitecoreInfoAttribute()
        {
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreInfoAttribute"/> class.
        /// </summary>
        /// <param name="infoType">Type of the info.</param>
        public SitecoreInfoAttribute(SitecoreInfoType infoType)
        {
            Type = infoType;
        }

        /// <summary>
        /// The type of information that should populate the property
        /// </summary>
        /// <value>The type.</value>
        public SitecoreInfoType Type { get; set; }

        /// <summary>
        /// UrlOptions, use in conjunction with SitecoreInfoType.Url
        /// </summary>
        /// <value>The URL options.</value>
        public SitecoreInfoUrlOptions UrlOptions { get; set; }

        /// <summary>
        /// MediaUrlOptions, use in conjunction with SitecoreInfoType.MediaUrl
        /// </summary>
        /// <value>The URL options.</value>
        public SitecoreInfoMediaUrlOptions MediaUrlOptions { get; set; }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns>AbstractPropertyConfiguration.</returns>
        public override Mapper.Configuration.AbstractPropertyConfiguration Configure(PropertyInfo propertyInfo)
        {
            var config = new SitecoreInfoConfiguration();
            Configure(propertyInfo, config);
            return config;
        }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="config">The config.</param>
        public void Configure(PropertyInfo propertyInfo, SitecoreInfoConfiguration config)
        {
            config.Type = Type;
            config.UrlOptions = UrlOptions;
            config.MediaUrlOptions = MediaUrlOptions;
            base.Configure(propertyInfo, config);
        }
    }
}




