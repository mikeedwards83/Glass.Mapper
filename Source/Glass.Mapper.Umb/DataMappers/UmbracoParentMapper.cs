using System;
using Glass.Mapper.Umb.Configuration;

namespace Glass.Mapper.Umb.DataMappers
{
    /// <summary>
    /// UmbracoParentMapper
    /// </summary>
    public class UmbracoParentMapper :AbstractDataMapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoParentMapper"/> class.
        /// </summary>
        public UmbracoParentMapper()
        {
            ReadOnly = true;
        }

        /// <summary>
        /// Maps data from the .Net property value to the CMS value
        /// </summary>
        /// <param name="mappingContext"></param>
        /// <exception cref="System.NotSupportedException"></exception>
        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Maps data from the CMS value to the .Net property value
        /// </summary>
        /// <param name="mappingContext"></param>
        /// <returns></returns>
        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var umbContext = mappingContext as UmbracoDataMappingContext;
            var umbConfig = Configuration as UmbracoParentConfiguration;

            return umbContext.Service.CreateType(
                umbConfig.PropertyInfo.PropertyType,
                umbContext.Service.ContentService.GetById(umbContext.Content.ParentId),
                umbConfig.IsLazy,
                umbConfig.InferType);
        }

        /// <summary>
        /// Indicates that the data mapper will mapper to and from the property
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is UmbracoParentConfiguration;
        }
    }
}
