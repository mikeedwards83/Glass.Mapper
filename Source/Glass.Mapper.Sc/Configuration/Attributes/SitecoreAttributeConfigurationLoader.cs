using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    /// <summary>
    /// Class SitecoreAttributeConfigurationLoader
    /// </summary>
    //[Obsolete("This class is no longer required, use Glass.Mapper.Configuration.Attributes.AttributeConfigurationLoader")]
    public class SitecoreAttributeConfigurationLoader : AttributeConfigurationLoader
    {
        /// <summary> 
        /// Initializes a new instance of the <see cref="SitecoreAttributeConfigurationLoader"/> class.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        public SitecoreAttributeConfigurationLoader(params string[] assemblies): base(assemblies)
        {

        }
    }
}




