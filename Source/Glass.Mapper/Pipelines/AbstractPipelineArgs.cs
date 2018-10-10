using System.Collections.Generic;

namespace Glass.Mapper.Pipelines
{
    /// <summary>
    /// Abstract class that acts as the base class for all arguments passed in pipelines
    /// </summary>
    public abstract class AbstractPipelineArgs
    {
        public Dictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        public Context Context { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is aborted.
        /// </summary>
        /// <value><c>true</c> if this instance is aborted; otherwise, <c>false</c>.</value>
       // public bool IsAborted { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractPipelineArgs"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        protected AbstractPipelineArgs(Context context)
        {
            Parameters = new Dictionary<string, object>();
            Context = context;
        }

        /// <summary>
        /// Aborts the pipeline.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        //public bool AbortPipeline ()
        //{
        //    IsAborted = true;
        //    return IsAborted;
        //}

       
    }
}




