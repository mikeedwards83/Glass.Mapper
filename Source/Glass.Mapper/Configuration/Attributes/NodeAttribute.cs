using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Glass.Mapper.Configuration.Attributes
{
    public abstract class NodeAttribute : AbstractPropertyAttribute
    {
        public NodeAttribute()
        {
            IsLazy = true;
        }

        /// <summary>
        /// Indicates that the item should be loaded lazily. Default value is true. If false the item will be loaded when the containing object is created.
        /// </summary>
        public bool IsLazy
        {
            get;
            set;
        }

        /// <summary>
        /// The path to the item. If both a path and ID are specified the ID will be used.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The Id of the item. 
        /// </summary>
        public string Id { get; set; }

        public void Configure(PropertyInfo propertyInfo, NodeConfiguration config)
        {
            config.Id = this.Id;
            config.IsLazy = this.IsLazy;
            config.Path = this.Path;
            
            base.Configure(propertyInfo, config);
        }
    }
}
