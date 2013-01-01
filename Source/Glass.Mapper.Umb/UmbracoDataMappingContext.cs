using umbraco.interfaces;

namespace Glass.Mapper.Umb
{
    public class UmbracoDataMappingContext : AbstractDataMappingContext
    {
        public UmbracoDataMappingContext(object obj, INode node, IUmbracoService service)
            : base(obj)
        {
            Node = node;
            Service = service;
        }

        public INode Node { get; private set; }

        public IUmbracoService Service { get; set; }
    }
}
