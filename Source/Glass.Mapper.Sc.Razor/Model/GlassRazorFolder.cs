using System;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace Glass.Mapper.Sc.Razor.Model
{
    /// <summary>
    /// Class GlassRazorFolder
    /// </summary>
    [SitecoreType]
    public class GlassRazorFolder
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [SitecoreId]
        public virtual Guid Id { get; set; }
    }
}
