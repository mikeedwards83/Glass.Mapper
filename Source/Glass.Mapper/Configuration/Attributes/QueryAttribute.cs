

using System.Reflection;

namespace Glass.Mapper.Configuration.Attributes
{
    /// <summary>
    /// Class QueryAttribute
    /// </summary>
    public abstract class QueryAttribute : AbstractPropertyAttribute
    {
        /// <summary>
        /// The query to execute
        /// </summary>
        /// <value>The query.</value>
        public string Query { get; set; }

        /// <summary>
        /// Indicates that the field is relative to the current item.
        /// </summary>
        /// <value><c>true</c> if this instance is relative; otherwise, <c>false</c>.</value>
        public bool IsRelative { get; set; }

        /// <summary>
        /// Indicates the type should be inferred from the item template
        /// </summary>
        /// <value><c>true</c> if [infer type]; otherwise, <c>false</c>.</value>
        public bool InferType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryAttribute"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        public QueryAttribute(string query)
        {
            Query = query;
        }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="config">The config.</param>
        public  void Configure(PropertyInfo propertyInfo, QueryConfiguration config)
        {
            config.Query = Query;
            config.IsRelative = IsRelative;
            config.InferType = InferType;

            base.Configure(propertyInfo, config);
        }

    }
}




