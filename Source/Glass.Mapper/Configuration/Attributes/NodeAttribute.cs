

using System.Reflection;

namespace Glass.Mapper.Configuration.Attributes
{
    /// <summary>
    /// Class NodeAttribute
    /// </summary>
    public abstract class NodeAttribute : AbstractPropertyAttribute
    {
        /// <summary>
        /// The path to the item. If both a path and ID are specified the ID will be used.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; set; }

        /// <summary>
        /// The Id of the item.
        /// </summary>
        /// <value>The id.</value>
        public string Id { get; set; }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="config">The config.</param>
        public void Configure(PropertyInfo propertyInfo, NodeConfiguration config)
        {
            config.Id = Id;
            config.Path = Path;
            
            base.Configure(propertyInfo, config);
        }
    }
}




