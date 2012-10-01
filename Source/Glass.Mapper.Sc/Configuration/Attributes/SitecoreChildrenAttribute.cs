using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    /// <summary>
    /// Indicates that the property should contain the children of the current item for the sitecore implementation
    /// </summary>
    public class SitecoreChildrenAttribute : ChildrenAttribute
    {
    }
}
