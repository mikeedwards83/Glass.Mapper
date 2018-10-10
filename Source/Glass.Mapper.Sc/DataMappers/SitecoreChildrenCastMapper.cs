using System;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Custom;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreChildrenCastMapper : AbstractDataMapper
    {
        private readonly LazyLoadingHelper _lazyLoadingHelper;

        public SitecoreChildrenCastMapper(LazyLoadingHelper lazyLoadingHelper)
        {
            _lazyLoadingHelper = lazyLoadingHelper;
            ReadOnly = true;
        }

        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            throw new NotImplementedException();
        }

        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var scContext = mappingContext as SitecoreDataMappingContext;
            var scConfig = Configuration as SitecoreChildrenConfiguration;

            if (scContext != null && scConfig != null)
            {
                var options = new GetItemOptions();
                options.Copy(mappingContext.Options);
                scConfig.GetPropertyOptions(options);

                return new ChildrenCast(scContext.Service, scContext.Item, options, _lazyLoadingHelper);
            }
            return null;
        }

        public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is SitecoreChildrenConfiguration &&
                   configuration.PropertyInfo.PropertyType == typeof (ChildrenCast);
        }
    }
}
