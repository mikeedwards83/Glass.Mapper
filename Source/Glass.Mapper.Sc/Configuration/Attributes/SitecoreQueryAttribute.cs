using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    public class SitecoreQueryAttribute : QueryAttribute
    {
        public SitecoreQueryAttribute(string query):base(query)
        {
        }

        /// <summary>
        /// Use the Sitecore.Data.Query.QueryContext when querying for data
        /// </summary>
        public bool UseQueryContext { get; set; }

        public override AbstractPropertyConfiguration Configure(PropertyInfo propertyInfo)
        {
            var config = new SitecoreQueryConfiguration();
            Configure(propertyInfo, config);
            return config;
        }
        public void Configure(PropertyInfo propertyInfo, SitecoreQueryConfiguration config)
        {
            config.UseQueryContext = this.UseQueryContext;
            base.Configure(propertyInfo, (QueryConfiguration) config);
        }
    }
}
