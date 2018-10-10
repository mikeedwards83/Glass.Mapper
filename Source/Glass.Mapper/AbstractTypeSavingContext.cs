using Glass.Mapper.Configuration;

namespace Glass.Mapper
{
    /// <summary>
    /// Class AbstractTypeSavingContext
    /// </summary>
    public abstract class AbstractTypeSavingContext
    {
        /// <summary>
        /// Gets or sets the config.
        /// </summary>
        /// <value>The config.</value>
        public AbstractTypeConfiguration Config { get; set; }
        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        /// <value>The object.</value>
        public object Object { get; set; }

        public abstract AbstractDataMappingContext CreateDataMappingContext(IAbstractService service);
    }
}




