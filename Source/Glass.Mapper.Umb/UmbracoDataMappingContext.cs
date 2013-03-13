using Umbraco.Core.Models;

namespace Glass.Mapper.Umb
{
    /// <summary>
    /// UmbracoDataMappingContext
    /// </summary>
    public class UmbracoDataMappingContext : AbstractDataMappingContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoDataMappingContext"/> class.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="content">The content.</param>
        /// <param name="service">The service.</param>
        public UmbracoDataMappingContext(object obj, IContent content, IUmbracoService service)
            : base(obj)
        {
            Content = content;
            Service = service;
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public IContent Content { get; set; }

        /// <summary>
        /// Gets or sets the service.
        /// </summary>
        /// <value>
        /// The service.
        /// </value>
        public IUmbracoService Service { get; set; }
    }
}
