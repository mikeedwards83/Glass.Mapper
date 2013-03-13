using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Core.Models;

namespace Glass.Mapper.Umb
{
    /// <summary>
    /// UmbracoTypeSavingContext
    /// </summary>
    public class UmbracoTypeSavingContext : AbstractTypeSavingContext
    {
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public IContent Content { get; set; }
    }
}
