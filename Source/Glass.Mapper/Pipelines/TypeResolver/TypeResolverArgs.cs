using System;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Pipelines.TypeResolver
{
    public class TypeResolverArgs : AbstractPipelineArgs
    {
        public AbstractTypeCreationContext AbstractTypeCreationContext { get; private set; }
        public Type Result { get; set; }

        public TypeResolverArgs(Context context, AbstractTypeCreationContext abstractTypeCreationContext):base(context)
        {
            AbstractTypeCreationContext = abstractTypeCreationContext;
        }

    }
}