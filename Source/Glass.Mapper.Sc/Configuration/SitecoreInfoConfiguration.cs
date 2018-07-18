using Glass.Mapper.Configuration;

namespace Glass.Mapper.Sc.Configuration
{
    /// <summary>
    /// Class SitecoreInfoConfiguration
    /// </summary>
    public class SitecoreInfoConfiguration : InfoConfiguration
    {
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


        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new SitecoreInfoConfiguration();
        }

        protected override void Copy(AbstractPropertyConfiguration copy)
        {
            var config = copy as SitecoreInfoConfiguration;
            config.Type = Type;
            config.UrlOptions = UrlOptions;
            config.MediaUrlOptions = MediaUrlOptions;
            base.Copy(copy);
        }
    }
}




