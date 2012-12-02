using System;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Pipelines.TypeResolver
{
    public class TypeResolverArgs : AbstractPipelineArgs
    {
        public ITypeContext TypeContext { get; private set; }
        public Type Result { get; set; }

        public TypeResolverArgs(Context context, ITypeContext typeContext):base(context)
        {
            TypeContext = typeContext;
        }

    }
}