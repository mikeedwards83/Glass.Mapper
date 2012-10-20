using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.DataMapperResolver;

namespace Glass.Mapper
{
    /// <summary>
    /// A data mapper converts data from the CMS stored value to the .Net data type
    /// </summary>
    public abstract class AbstractDataMapper : IDataMapperResolverTask
    {
        /// <summary>
        /// The property this Data Mapper will populate
        /// </summary>
        public System.Reflection.PropertyInfo Property { get; set; }

        /// <summary>
        /// Takes CMS data and writes it to the property
        /// </summary>
        /// <param name="mappingContext"></param>
        public void MapCmsToProperty(AbstractDataMappingContext mappingContext)
        {

            var result  = MapFromCms(mappingContext);

            //TODO: see if this can be sped up, I suspect dynamic IL would be quicker
            if (result != null)
                Property.SetValue(mappingContext.Object, result, null);
        }

        /// <summary>
        /// Maps data from the .Net property value to the CMS value
        /// </summary>
        /// <param name="mappingContext"></param>
        /// <returns>The value to write </returns>
        public abstract object MapToCms(AbstractDataMappingContext mappingContext);

        /// <summary>
        /// Maps data from the CMS value to the .Net property value
        /// </summary>
        /// <param name="mappingContext"></param>
        /// <returns></returns>
        public abstract object MapFromCms(AbstractDataMappingContext mappingContext);


        /// <summary>
        /// Sets up the data mapper for a particular property
        /// </summary>
        /// <param name="configuration"></param>
        public abstract void Setup(AbstractPropertyConfiguration configuration);


        /// <summary>
        /// Indicates that the data mapper will mapper to and from the property
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public abstract bool CanHandle(AbstractPropertyConfiguration configuration);

        /// <summary>
        /// Called by the DataMapperResolver pipeline to determine if this data mapper should handle the property
        /// </summary>
        /// <param name="args"></param>
        public virtual void Execute(DataMapperResolverArgs args)
        {
            if (args.IsAborted || args.Result != null)
                return;

            bool canHandle = CanHandle(args.PropertyConfiguration);
            
            if(canHandle)
            {
                args.Result = this;
                args.AbortPipeline();
            }
        }

        
    }
}
