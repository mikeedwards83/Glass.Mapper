using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper.Sc.Pipelines.Response
{
    public class ChainedViewTypeResolver : IViewTypeResolver
    {
        private readonly IEnumerable<IViewTypeResolver> _resolvers;
        public ChainedViewTypeResolver(IEnumerable<IViewTypeResolver> resolvers)
        {
            _resolvers = resolvers;
        }

        public Type GetType(string path)
        {
            var found = typeof(NullModel);
            foreach (var resolver in _resolvers)
            {
                if (found != typeof(NullModel))
                {
                    break;
                }
                found = resolver.GetType(path);
            }
            return found;
        }
    }
}
