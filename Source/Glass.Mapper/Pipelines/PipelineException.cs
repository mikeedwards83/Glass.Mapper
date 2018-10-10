
using System;

namespace Glass.Mapper.Pipelines
{
    /// <summary>
    /// Class PipelineException
    /// </summary>
    public class PipelineException : ApplicationException
    {
        /// <summary>
        /// Gets the args.
        /// </summary>
        /// <value>The args.</value>
        public AbstractPipelineArgs Args { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The args.</param>
        public PipelineException(string message, AbstractPipelineArgs args)
        {
            Args = args;
        }
    }
}




