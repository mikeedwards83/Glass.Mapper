

namespace Glass.Mapper.Pipelines.ObjectSaving
{
    /// <summary>
    /// Class ObjectSavingArgs
    /// </summary>
    public class ObjectSavingArgs : AbstractPipelineArgs
    {
        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <value>The target.</value>
        public object Target { get; private set; }
        /// <summary>
        /// Gets the saving context.
        /// </summary>
        /// <value>The saving context.</value>
        public AbstractTypeSavingContext SavingContext { get; private set; }
        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <value>The service.</value>
        public IAbstractService Service { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectSavingArgs"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The target.</param>
        /// <param name="savingContext">The saving context.</param>
        /// <param name="service">The service.</param>
        public ObjectSavingArgs(
            Context context, 
            object target, 
            AbstractTypeSavingContext savingContext,
            IAbstractService service)
            : base(context)
        {
            Target = target;
            SavingContext = savingContext;
            Service = service;
        }
    }
}




