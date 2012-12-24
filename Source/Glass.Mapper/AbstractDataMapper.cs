using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.DataMapperResolver;

namespace Glass.Mapper
{

    /***** 
    /// 
    /// I have assumed that all CMSs at the moment will using Strings for data storage
    /// 
    *****/ 
    
    /// <summary>
    /// A data mapper converts data from the CMS stored value to the .Net data type
    /// </summary>
    public abstract class AbstractDataMapper
    {
        public bool ReadOnly { get;  set; }

        /// <summary>
        /// The property this Data Mapper will populate
        /// </summary>
        public AbstractPropertyConfiguration Configuration { get; private set; }

        /// <summary>
        /// Takes CMS data and writes it to the property
        /// </summary>
        /// <param name="mappingContext"></param>
        public void MapCmsToProperty(AbstractDataMappingContext mappingContext)
        {
            var result  = MapToProperty(mappingContext);

            //TODO: see if this can be sped up, I suspect dynamic IL would be quicker
            if (result != null)
               Configuration.PropertyInfo.SetValue(mappingContext.Object, result, null);
        }

        /// <summary>
        /// Takes a Property value and writes it to a CMS value
        /// </summary>
        /// <param name="mappingContext"></param>
        public void MapPropertyToCms(AbstractDataMappingContext mappingContext)
        {
            if (ReadOnly) return;

            //TODO: see if this can be sped up, I suspect dynamic IL would be quicker
            mappingContext.PropertyValue = Configuration.PropertyInfo.GetValue(mappingContext.Object, null);
            MapToCms(mappingContext);
        }



        /// <summary>
        /// Maps data from the .Net property value to the CMS value
        /// </summary>
        /// <param name="mappingContext"></param>
        /// <returns>The value to write </returns>
        public abstract void MapToCms(AbstractDataMappingContext mappingContext);

        /// <summary>
        /// Maps data from the CMS value to the .Net property value
        /// </summary>
        /// <param name="mappingContext"></param>
        /// <returns></returns>
        public abstract object MapToProperty(AbstractDataMappingContext mappingContext);


        /// <summary>
        /// Sets up the data mapper for a particular property
        /// </summary>
        /// <param name="configuration"></param>
        public virtual void Setup(DataMapperResolverArgs args)
        {
            Configuration = args.PropertyConfiguration;
        }


        /// <summary>
        /// Indicates that the data mapper will mapper to and from the property
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public abstract bool CanHandle(AbstractPropertyConfiguration configuration, Context context);

        
    }
}
