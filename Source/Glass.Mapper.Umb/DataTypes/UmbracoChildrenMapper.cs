using System;
using System.Collections.Generic;
using Glass.Mapper.Umb.Configuration;
using umbraco.NodeFactory;
using umbraco.interfaces;

namespace Glass.Mapper.Umb.DataTypes
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
            
            Func<IEnumerable<INode>> getItems = () => umbContext.Node.ChildrenAsList;

            return Utilities.CreateGenericType(
                typeof(LazyNodeEnumerable<>),
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
