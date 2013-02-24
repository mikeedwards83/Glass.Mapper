using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Glass.Mapper.Umb
{
    public class UmbracoDataMappingContext : AbstractDataMappingContext
    {
        public UmbracoDataMappingContext(object obj, IContent content, IUmbracoService service, IContentService contentService)
            : base(obj)
        {
            Content = content;
            Service = service;
            ContentService = contentService;
        }

        public IContent Content { get; set; }

        public IUmbracoService Service { get; set; }

        public IContentService ContentService { get; set; }
    }
}
