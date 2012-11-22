using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Glass.Mapper.Configuration.Attributes
{
    public abstract class QueryAttribute : AbstractPropertyAttribute
    {
        /// <summary>
        /// The query to execute
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Indicates that the results should be loaded lazily
        /// </summary>
        public bool IsLazy { get; set; }

        /// <summary>
        /// Indicates that the field is relative to the current item.
        /// </summary>
        public bool IsRelative { get; set; }

        /// <summary>
        /// Indicates the type should be inferred from the item template
        /// </summary>
        public bool InferType { get; set; }

        public QueryAttribute(string query)
        {
            IsLazy = true;
            Query = query;
        }

        public  void Configure(PropertyInfo propertyInfo, QueryConfiguration config)
        {
            config.Query = this.Query;
            config.IsLazy = this.IsLazy;
            config.IsRelative = this.IsRelative;
            config.InferType = this.InferType;

            base.Configure(propertyInfo, config);
        }

    }
}
