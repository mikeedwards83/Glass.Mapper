using Glass.Mapper.Umb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Core.Models;

namespace Glass.Mapper.Umb
{
    /// <summary>
    /// UmbracoTypeCreationContext
    /// </summary>
    public class UmbracoTypeCreationContext : AbstractTypeCreationContext
    {
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public IContent Content { get; set; }

        /// <summary>
        /// Gets or sets the umbraco service.
        /// </summary>
        /// <value>
        /// The umbraco service.
        /// </value>
        public IUmbracoService UmbracoService { get; set; }
    }
}
