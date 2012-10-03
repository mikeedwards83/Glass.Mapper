using System;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Pipelines.TypeResolver
{
    public class TypeResolverArgs : AbstractPipelineArgs
    {
        /// <summary>
        /// Indicates that the type should be inferred
        /// </summary>
        public bool InferType { get; set; }

        /// <summary>
        /// Indicates that the type was requested via a property
        /// </summary>
        public bool IsProperty { get; set; }

        /// <summary>
        /// The configuration of the property to load
        /// </summary>
        public AbstractPropertyConfiguration Configuration { get; set; }

        /// <summary>
        /// The type to create
        /// </summary>
        public Type FinalType { get; set; }

        /// <summary>
        /// The originally requested type
        /// </summary>
        public Type RequestedType { get; set; }
    }
}