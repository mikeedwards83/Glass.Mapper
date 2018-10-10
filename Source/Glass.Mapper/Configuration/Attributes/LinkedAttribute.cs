using System.Reflection;

namespace Glass.Mapper.Configuration.Attributes
{
    /// <summary>
    /// Class LinkedAttribute
    /// </summary>
    public abstract class LinkedAttribute : AbstractPropertyAttribute
    {
        /// <summary>
        /// Indicates the type should be inferred from the item template
        /// </summary>
        /// <value><c>true</c> if [infer type]; otherwise, <c>false</c>.</value>
        public bool InferType { get; set; }

        /// <summary>
        /// Configures the specified info.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="config">The config.</param>
        public void Configure(PropertyInfo info, LinkedConfiguration config)
        {
            config.InferType = InferType;
            base.Configure(info, config);
        }
    }
}




