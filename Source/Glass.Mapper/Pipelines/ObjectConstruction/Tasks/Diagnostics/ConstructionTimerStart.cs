using System.Diagnostics;
using Glass.Mapper.Caching;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.Diagnostics
{
    public class ConstructionTimerStart : IObjectConstructionTask
    {
        private ICacheKeyGenerator _cacheKeyGenerator;

        public ConstructionTimerStart(ICacheKeyGenerator cacheKeyGenerator)
        {
            _cacheKeyGenerator = cacheKeyGenerator;
        }

        public void Execute(ObjectConstructionArgs args)
        {
            var key = _cacheKeyGenerator.Generate(args) + "stopwatch";
            var stopwatch = new Stopwatch();

            args.Parameters[key] = stopwatch;
            stopwatch.Start();

        }
    }
}
