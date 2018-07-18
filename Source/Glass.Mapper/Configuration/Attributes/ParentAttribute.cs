using System.Reflection;

namespace Glass.Mapper.Configuration.Attributes
{
    /// <summary>
    /// Class ParentAttribute
    /// </summary>
    public abstract class ParentAttribute : AbstractPropertyAttribute
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ParentAttribute"/> class.
        /// </summary>
        public ParentAttribute()
        {
            InferType = false;
        }
        
        /// <summary>
        /// Indicates the type should be inferred from the item template
        /// </summary>
        /// <value><c>true</c> if [infer type]; otherwise, <c>false</c>.</value>
        public bool InferType
        {
            get;
            set;
        }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="config">The config.</param>
        public void Configure(PropertyInfo propertyInfo, ParentConfiguration config)
        {
            config.InferType = InferType;

            base.Configure(propertyInfo, config);
        }
    }
}




