using System;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Pipelines.TypeResolver
{
    public class TypeResolverArgs : AbstractPipelineArgs
    {
        public IDataContext DataContext { get; private set; }
        public Type Result { get; set; }

        public TypeResolverArgs(Context context, IDataContext dataContext):base(context)
        {
            DataContext = dataContext;
        }

    }
}