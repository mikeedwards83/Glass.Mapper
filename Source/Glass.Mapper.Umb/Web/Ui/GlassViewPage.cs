using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;

namespace Glass.Mapper.Umb.Web.Ui
{
    /// <summary>
    /// GlassViewPage
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GlassViewPage<T> : UmbracoViewPage<T> where T : class
    {
        private readonly IUmbracoService _umbracoService;


        /// <summary>
        /// Initializes a new instance of the <see cref="GlassViewPage{T}"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public GlassViewPage(IUmbracoService service)
        {
            _umbracoService = service;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlassViewPage{T}"/> class.
        /// </summary>
        public GlassViewPage()
            : this(new UmbracoService(new ContentService()))
        {
        }

        /// <summary>
        /// Gets the umbraco service.
        /// </summary>
        /// <value>
        /// The umbraco service.
        /// </value>
        public IUmbracoService UmbracoService
        {
            get { return _umbracoService; }
        }

        /// <summary>
        /// Executes the server code in the current web page that is marked using Razor syntax.
        /// </summary>
        public override void Execute()
        {
        }
    }
}
