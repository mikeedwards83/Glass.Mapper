


using System.Reflection;

namespace Glass.Mapper.Configuration.Attributes
{
    /// <summary>
    /// Class FieldAttribute
    /// </summary>
    public abstract class FieldAttribute : AbstractPropertyAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldAttribute"/> class.
        /// </summary>
        public FieldAttribute()
        { }

        /// <summary>
        /// When true the field will not be save back
        /// </summary>
        /// <value><c>true</c> if [read only]; otherwise, <c>false</c>.</value>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="config">The config.</param>
        public void Configure(PropertyInfo propertyInfo, FieldConfiguration config)
        {
            config.ReadOnly = ReadOnly;
            base.Configure(propertyInfo, config);
        }
    }
}




