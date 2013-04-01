using System;
using System.Collections.Generic;
using Glass.Mapper.Umb.Configuration;
using Umbraco.Core.Models;

namespace Glass.Mapper.Umb.DataMappers
{
    /// <summary>
    /// UmbracoChildrenMapper
    /// </summary>
    public class UmbracoChildrenMapper : AbstractDataMapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoChildrenMapper"/> class.
        /// </summary>
        public UmbracoChildrenMapper()
        {
            ReadOnly = true;
        }

        /// <summary>
        /// Maps data from the .Net property value to the CMS value
        /// </summary>
        /// <param name="mappingContext"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Maps data from the CMS value to the .Net property value
        /// </summary>
        /// <param name="mappingContext"></param>
        /// <returns></returns>
        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var umbContext = mappingContext as UmbracoDataMappingContext;
            var umbConfig = Configuration as UmbracoChildrenConfiguration;

            Type genericType = Utilities.GetGenericArgument(Configuration.PropertyInfo.PropertyType);
            
            Func<IEnumerable<IContent>> getItems = () => umbContext.Service.ContentService.GetChildren(umbContext.Content.Id);

            return Utilities.CreateGenericType(
                typeof(LazyContentEnumerable<>),
                new[] {genericType},
                getItems,
                umbConfig.IsLazy,
                umbConfig.InferType,
                umbContext.Service
                );
        }

        /// <summary>
        /// Indicates that the data mapper will mapper to and from the property
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration,  Context context)
        {
            return configuration is UmbracoChildrenConfiguration;
        }
    }
}
