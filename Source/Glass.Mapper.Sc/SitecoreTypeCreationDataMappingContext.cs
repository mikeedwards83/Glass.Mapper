namespace Glass.Mapper.Sc
{
    public class SitecoreTypeCreationDataMappingContext : SitecoreDataMappingContext
    {
        public SitecoreTypeCreationDataMappingContext(object obj, SitecoreTypeCreationContext typeCreationContext, ISitecoreService service)
            : base(obj, typeCreationContext.Item, service)
        {
            TypeCreationContext = TypeCreationContext;
        }

        public SitecoreTypeCreationContext TypeCreationContext { get; private set; }
    }
}