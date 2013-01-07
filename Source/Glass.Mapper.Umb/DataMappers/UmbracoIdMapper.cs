using System;
using Glass.Mapper.Umb.Configuration;

namespace Glass.Mapper.Umb.DataMappers
{
    public class UmbracoIdMapper : AbstractDataMapper
    {
        public UmbracoIdMapper()
        {
            this.ReadOnly = true;
        }

        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            throw new NotImplementedException();
        }

        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            UmbracoDataMappingContext context = mappingContext as UmbracoDataMappingContext;
            var node = context.Node;

            var umbConfig = Configuration as UmbracoIdConfiguration;

            if (umbConfig.PropertyInfo.PropertyType == typeof (int))
                return node.Id;
            
            throw new NotSupportedException("The type {0} on {0}.{1} is not supported by UmbracoIdMapper".Formatted
                                                (umbConfig.PropertyInfo.ReflectedType.FullName,
                                                 umbConfig.PropertyInfo.Name));
        }

        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is UmbracoIdConfiguration;
        }
    }
}
