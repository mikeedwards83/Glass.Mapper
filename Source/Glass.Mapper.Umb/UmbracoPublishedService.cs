using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Services;

namespace Glass.Mapper.Umb
{
    public class UmbracoPublishedService : UmbracoService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoService"/> class.
        /// </summary>
        /// <param name="contentService">The content service.</param>
        /// <param name="contextName">Name of the context.</param>
        public UmbracoPublishedService(IContentService contentService, string contextName = "Default")
            :base(contentService, contextName)
        {
            this.PublishedOnly = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoService"/> class.
        /// </summary>
        /// <param name="contentService">The content service.</param>
        /// <param name="context">The context.</param>
        public UmbracoPublishedService(IContentService contentService, Context context)
            : base(contentService, context)
        {
            this.PublishedOnly = true;
        }
    }
}
