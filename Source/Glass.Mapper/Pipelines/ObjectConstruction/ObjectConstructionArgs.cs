using Glass.Mapper.Configuration;
using Glass.Mapper.Diagnostics;

namespace Glass.Mapper.Pipelines.ObjectConstruction
{
    /// <summary>
    /// Class ObjectConstructionArgs
    /// </summary>
    public class ObjectConstructionArgs : AbstractPipelineArgs
    {
        /// <summary>
        /// Context of the item being created
        /// </summary>
        /// <value>The abstract type creation context.</value>
        public AbstractTypeCreationContext AbstractTypeCreationContext { get; private set; }

        /// <summary>
        /// The configuration to use to load the object
        /// </summary>
        /// <value>The configuration.</value>
        public AbstractTypeConfiguration Configuration { get; private set; }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <value>The service.</value>
        public IAbstractService Service { get; private set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        public object Result
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectConstructionArgs"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="abstractTypeCreationContext">The abstract type creation context.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="service">The service.</param>
        public ObjectConstructionArgs(
            Context context,
            AbstractTypeCreationContext abstractTypeCreationContext,
            AbstractTypeConfiguration configuration,
            IAbstractService service)
            : base(context)
        {
            AbstractTypeCreationContext = abstractTypeCreationContext;
            Configuration = configuration;
            Service = service;
        }


        public GetOptions Options
        {
            get { return AbstractTypeCreationContext.Options; }
        }

    }
}




