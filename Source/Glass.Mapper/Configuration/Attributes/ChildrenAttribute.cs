

namespace Glass.Mapper.Configuration.Attributes
{
    /// <summary>
    /// Indicates that the property should contain the children of the current item
    /// </summary>
    public abstract class ChildrenAttribute : AbstractPropertyAttribute
    {
        /// <summary>
        /// Indicates the type should be inferred from the item template
        /// </summary>
        /// <value><c>true</c> if [infer type]; otherwise, <c>false</c>.</value>
        public virtual bool InferType 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="config">The config.</param>
        public void Configure(System.Reflection.PropertyInfo propertyInfo, ChildrenConfiguration config)
        {
            config.InferType = InferType;
            base.Configure(propertyInfo, config);
        }
    }
}




