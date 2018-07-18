using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// Class SitecoreIdMapper
    /// </summary>
    public class SitecoreIdMapper : AbstractDataMapper
    {
        private Func<Item, object> _getValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreIdMapper"/> class.
        /// </summary>
        public SitecoreIdMapper()
        {
            this.ReadOnly = true;
        }

        /// <summary>
        /// Maps data from the .Net property value to the CMS value
        /// </summary>
        /// <param name="mappingContext">The mapping context.</param>
        /// <returns>The value to write</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Maps data from the CMS value to the .Net property value
        /// </summary>
        /// <param name="mappingContext">The mapping context.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.NotSupportedException">The type {0} on {0}.{1} is not supported by SitecoreIdMapper.Formatted
        ///                                                     (scConfig.PropertyInfo.ReflectedType.FullName,
        ///                                                         scConfig.PropertyInfo.Name)</exception>
        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            SitecoreDataMappingContext context = (SitecoreDataMappingContext)mappingContext;
            var item = context.Item;
            return _getValue(item);
        }

        public override void Setup(DataMapperResolverArgs args)
        {
            if (args.PropertyConfiguration.PropertyInfo.PropertyType == typeof(Guid))
                _getValue = (item) => item.ID.Guid;
            else if (args.PropertyConfiguration.PropertyInfo.PropertyType == typeof(ID))
                _getValue = (item) => item.ID;
            else
            {
                throw new NotSupportedException("The type {0} on {0}.{1} is not supported by SitecoreIdMapper".Formatted
                                                    (args.PropertyConfiguration.PropertyInfo.ReflectedType.FullName,
                                                        args.PropertyConfiguration.PropertyInfo.Name));
            }

            base.Setup(args);
        }


        /// <summary>
        /// Indicates that the data mapper will mapper to and from the property
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if this instance can handle the specified configuration; otherwise, <c>false</c>.</returns>
        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is SitecoreIdConfiguration;
        }
    }
}




