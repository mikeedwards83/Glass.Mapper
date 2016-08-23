/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Umbraco.Core.Services;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Glass.Mapper.Umb.Web.Ui
{
    /// <summary>
    /// GlassTemplatePage
    /// </summary>
    public class GlassTemplatePage : UmbracoTemplatePage
    {
        private readonly IUmbracoService _umbracoService;

        /// <summary>
        /// Gets or sets the render model.
        /// </summary>
        /// <value>
        /// The render model.
        /// </value>
        public RenderModel RenderModel { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlassTemplatePage{T}"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public GlassTemplatePage(IUmbracoService service)
        {
            _umbracoService = service;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlassTemplatePage{T}"/> class.
        /// </summary>
        public GlassTemplatePage()
            : this(new UmbracoPublishedService(new ContentService()))
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
        /// Initializes the page.
        /// </summary>
        protected override void InitializePage()
        {
            base.InitializePage();
            RenderModel = Model;
        }

        /// <summary>
        /// Executes the server code in the current web page that is marked using Razor syntax.
        /// </summary>
        public override void Execute()
        {
        }
    }

    /// <summary>
    /// GlassTemplatePage
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GlassTemplatePage<T> : GlassTemplatePage where T : class
    {
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public new T Model { get; set; }
        
        /// <summary>
        /// Initializes the page.
        /// </summary>
        protected override void InitializePage()
        {
            base.InitializePage();
            Model = UmbracoService.CreateType<T>(UmbracoService.ContentService.GetPublishedVersion(base.Umbraco.AssignedContentItem.Id));
        }

        /// <summary>
        /// Executes the server code in the current web page that is marked using Razor syntax.
        /// </summary>
        public override void Execute()
        {
        }
    }
}

