namespace Glass.Mapper
{
    /// <summary>
    /// Represents the context when a CMS value is mapper to/from a .Net property value
    /// </summary>
    public class AbstractDataMappingContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractDataMappingContext"/> class.
        /// </summary>
        /// <param name="obj">The obj.</param>
        public AbstractDataMappingContext(object obj, GetOptions options)
        {
            Object = obj;
            Options = options;
        }


        public GetOptions Options { get; set; }



        /// <summary>
        /// Value stored by the CMS
        /// </summary>
        /// <value>The CMS value.</value>
        public string CmsValue { get; set; }

        /// <summary>
        /// Value stored by the Property
        /// </summary>
        /// <value>The property value.</value>
        public object PropertyValue { get; set; }

        /// <summary>
        /// The object containing the property being mapped
        /// </summary>
        /// <value>The object.</value>
        public object Object { get; private set; }
    }
}




