using System.Reflection;
using Glass.Mapper.Configuration;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Umb.Configuration.Attributes
{
    public class UmbracoParentAttribute : ParentAttribute
    {
        public override AbstractPropertyConfiguration Configure(PropertyInfo propertyInfo)
        {
            var config = new UmbracoParentConfiguration();
            Configure(propertyInfo, config);
            return config;
        }

        public void Configure(PropertyInfo propertyInfo, UmbracoParentConfiguration config)
        {
            base.Configure(propertyInfo, config);
        }
    }
}
