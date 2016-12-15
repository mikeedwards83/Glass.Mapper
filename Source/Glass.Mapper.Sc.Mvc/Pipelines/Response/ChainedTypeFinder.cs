using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper.Sc.Pipelines.Response
{
    public class ChainedTypeFinder : ITypeFinder
    {
        private readonly IEnumerable<ITypeFinder> _finders;
        public ChainedTypeFinder(IEnumerable<ITypeFinder> finders)
        {
            _finders = finders;
        }

        public Type GetType(string path)
        {
            var found = typeof(NullModel);
            foreach (var finder in _finders)
            {
                if (found != typeof(NullModel))
                {
                    break;
                }
                found = finder.GetType(path);
            }
            return found;
        }
    }
}
