using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectSaving;

namespace Glass.Mapper.Umb
{
    public class UmbracoObjectFactory : AbstractObjectFactory
    {
        public UmbracoObjectFactory(Context glassContext, ObjectConstruction objectConstruction, ConfigurationResolver configurationResolver, ObjectSaving objectSaving) 
            : base(glassContext, objectConstruction, configurationResolver, objectSaving)
        {
        }

        /// <summary>
        /// Creates the data mapping context.
        /// </summary>
        /// <param name="abstractTypeCreationContext">The abstract type creation context.</param>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public override AbstractDataMappingContext CreateDataMappingContext(AbstractTypeCreationContext abstractTypeCreationContext, Object obj)
        {
            var umbTypeContext = abstractTypeCreationContext as UmbracoTypeCreationContext;
            return new UmbracoDataMappingContext(obj, umbTypeContext.Content, umbTypeContext.Service as IUmbracoService);
        }

        /// <summary>
        /// Used to create the context used by DataMappers to map data from a class
        /// </summary>
        /// <param name="creationContext"></param>
        /// <returns></returns>
        public override AbstractDataMappingContext CreateDataMappingContext(AbstractTypeSavingContext creationContext)
        {
            var umbContext = creationContext as UmbracoTypeSavingContext;
            return new UmbracoDataMappingContext(umbContext.Object, umbContext.Content, umbContext.Service as IUmbracoService);
        }
    }
}
