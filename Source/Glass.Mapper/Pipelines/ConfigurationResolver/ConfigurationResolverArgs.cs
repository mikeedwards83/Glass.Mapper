using Glass.Mapper.Configuration;

namespace Glass.Mapper.Pipelines.ConfigurationResolver
{
    /// <summary>
    /// Class ConfigurationResolverArgs
    /// </summary>
    public class ConfigurationResolverArgs : AbstractPipelineArgs
    {
        public IAbstractService Service { get; set; }

        /// <summary>
        /// Gets the abstract type creation context.
        /// </summary>
        /// <value>The abstract type creation context.</value>
        public AbstractTypeCreationContext AbstractTypeCreationContext { get; private set; }
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        public AbstractTypeConfiguration Result { get; set; }


        public GetOptions Options { get { return AbstractTypeCreationContext.Options; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationResolverArgs"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="abstractTypeCreationContext">The abstract type creation context.</param>
        public ConfigurationResolverArgs(Context context,  AbstractTypeCreationContext abstractTypeCreationContext, IAbstractService service) :base(context)
        {
            Service = service;
            AbstractTypeCreationContext = abstractTypeCreationContext;
        }
    }
}




