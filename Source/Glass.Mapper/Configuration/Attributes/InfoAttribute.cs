using System.Reflection;

namespace Glass.Mapper.Configuration.Attributes
{
    /// <summary>
    /// Class InfoAttribute
    /// </summary>
    public abstract class  InfoAttribute : AbstractPropertyAttribute
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




