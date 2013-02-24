using System;
using System.Collections.Generic;
using Glass.Mapper.Umb.Configuration;
using Umbraco.Core.Models;

namespace Glass.Mapper.Umb.DataMappers
{
    public class UmbracoChildrenMapper : AbstractDataMapper
    {
        public UmbracoChildrenMapper()
        {
            this.ReadOnly = true;
        }

        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            throw new NotImplementedException();
        }

        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var umbContext = mappingContext as UmbracoDataMappingContext;
            var umbConfig = Configuration as UmbracoChildrenConfiguration;

            Type genericType = Utilities.GetGenericArgument(Configuration.PropertyInfo.PropertyType);
            
            Func<IEnumerable<IContent>> getItems = () => umbContext.ContentService.GetChildren(umbContext.Content.Id);

            return Utilities.CreateGenericType(
                typeof(LazyContentEnumerable<>),
                new[] {genericType},
                getItems,
                umbConfig.PropertyInfo.PropertyType,
                umbConfig.IsLazy,
                umbConfig.InferType,
                umbContext.Service
                );
        }

        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration,  Context context)
        {
            throw new NotImplementedException();
        }
    }
}
