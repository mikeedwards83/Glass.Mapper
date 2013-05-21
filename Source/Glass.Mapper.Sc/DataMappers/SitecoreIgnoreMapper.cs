using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// This mapped does nothing, used to ignore a property
    /// </summary>
     public class SitecoreIgnoreMapper : AbstractDataMapper
    {
        /// <summary>
        /// Takes CMS data and writes it to the property
        /// </summary>
        /// <param name="mappingContext">The mapping context.</param>
         public override void MapCmsToProperty(AbstractDataMappingContext mappingContext)
         {
             //does nothing!!!
         }

         /// <summary>
         /// Takes a Property value and writes it to a CMS value
         /// </summary>
         /// <param name="mappingContext">The mapping context.</param>
         public override void MapPropertyToCms(AbstractDataMappingContext mappingContext)
         {
             //does nothing!!!
         }

         /// <summary>
         /// Maps data from the .Net property value to the CMS value
         /// </summary>
         /// <param name="mappingContext">The mapping context.</param>
         /// <exception cref="System.NotImplementedException"></exception>
        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Maps data from the CMS value to the .Net property value
        /// </summary>
        /// <param name="mappingContext">The mapping context.</param>
        /// <returns>
        /// System.Object.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Indicates that the data mapper will mapper to and from the property
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if this instance can handle the specified configuration; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is IgnoreConfiguration;
        }
    }
}
