using System;
using RazorEngine.Templating;

namespace Glass.Mapper.Sc.Razor
{

    /// <summary>
    /// CachedView
    /// </summary>
    public class CachedView
    {
        /// <summary>
        /// Gets or sets the content of the view.
        /// </summary>
        /// <value>
        /// The content of the view.
        /// </value>
        public string ViewContent { get; set; }

        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        public ITemplate Template { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
    }
}
