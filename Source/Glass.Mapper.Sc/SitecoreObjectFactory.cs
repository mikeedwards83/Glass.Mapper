using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectSaving;

namespace Glass.Mapper.Sc
{
    public class SitecoreObjectFactory : AbstractObjectFactory
    {
        public SitecoreObjectFactory(Context context, ObjectConstruction objectConstruction, ConfigurationResolver configurationResolver, ObjectSaving objectSaving)
            : base(context, objectConstruction, configurationResolver, objectSaving)
        {
        }

        /// <summary>
        /// Creates the data mapping context.
        /// </summary>
        /// <param name="abstractTypeCreationContext">The abstract type creation context.</param>
        /// <param name="obj">The obj.</param>
        /// <returns>AbstractDataMappingContext.</returns>
        public override AbstractDataMappingContext CreateDataMappingContext(AbstractTypeCreationContext abstractTypeCreationContext, Object obj)
        {
            var scTypeContext = abstractTypeCreationContext as SitecoreTypeCreationContext;
            return new SitecoreDataMappingContext(obj, scTypeContext.Item, scTypeContext.Service as ISitecoreService);
        }

        /// <summary>
        /// Used to create the context used by DataMappers to map data from a class
        /// </summary>
        /// <param name="creationContext">The Saving Context</param>
        /// <returns>AbstractDataMappingContext.</returns>
        public override AbstractDataMappingContext CreateDataMappingContext(AbstractTypeSavingContext creationContext)
        {
            var scContext = creationContext as SitecoreTypeSavingContext;
            return new SitecoreDataMappingContext(scContext.Object, scContext.Item, scContext.Service as ISitecoreService);
        }
    }
}
