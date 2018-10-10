using System.Collections.Generic;

namespace Glass.Mapper
{
    /// <summary>
    /// The base class for the context loading an item from the CMS
    /// </summary>
    public abstract class AbstractTypeCreationContext
    {
        public AbstractTypeCreationContext()
        {
            Parameters = new Dictionary<string, object>();
        }
        /// <summary>
        /// ConstructorParameters that will be passed to the pipelines
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; }

        public abstract bool CacheEnabled { get; }


        public GetOptions Options { get; set; }

        public virtual string DataSummary()
        {
            return string.Empty;
        }

        public abstract AbstractDataMappingContext CreateDataMappingContext(object obj);

    }
}



