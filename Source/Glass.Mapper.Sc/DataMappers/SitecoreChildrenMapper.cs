using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreChildrenMapper : AbstractDataMapper
    {
        public SitecoreChildrenMapper()
        {
            this.ReadOnly = true;
        }

        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            throw new NotImplementedException();
        }

        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var scContext = mappingContext as SitecoreDataMappingContext;
            var scConfig = Configuration as SitecoreChildrenConfiguration;

            Type genericType = Utilities.GetGenericArgument(Configuration.PropertyInfo.PropertyType);

            Func<IEnumerable<Item>> getItems = () => scContext.Item.Children;

            return Utilities.CreateGenericType(
                typeof (LazyItemEnumerable<>),
                new[] {genericType},
                getItems,
                scConfig.PropertyInfo.PropertyType,
                scConfig.IsLazy,
                scConfig.InferType,
                scContext.Service
                );

        }

        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration,  Context context)
        {
            throw new NotImplementedException();
        }
    }
}
