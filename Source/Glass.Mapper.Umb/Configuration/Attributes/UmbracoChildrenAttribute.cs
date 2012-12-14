using System.Reflection;
using Glass.Mapper.Configuration;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Umb.Configuration.Attributes
{
    /// <summary>
    /// Indicates that the property should contain the children of the current item for the umbraco implementation
    /// </summary>
    public class UmbracoChildrenAttribute : ChildrenAttribute
    {
        public override AbstractPropertyConfiguration Configure(PropertyInfo propertyInfo)
        {
            var config = new UmbracoChildrenConfiguration();
            base.Configure(propertyInfo, config);
            return config;
        }
    }
}
