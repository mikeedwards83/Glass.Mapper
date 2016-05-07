using System.Diagnostics;
using Glass.Mapper.Caching;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.Diagnostics
{
    public class ConstructionTimerEnd : IObjectConstructionTask
    {
        private ICacheKeyGenerator _cacheKeyGenerator;
        private readonly Config.DebugSettings _debugSettings;
        private readonly ILog _log;


        public ConstructionTimerEnd(
            ICacheKeyGenerator cacheKeyGenerator, 
            Config.DebugSettings debugSettings,
            ILog log)
        {
            _cacheKeyGenerator = cacheKeyGenerator;
            _debugSettings = debugSettings;
            _log = log;
        }

        public void Execute(ObjectConstructionArgs args)
        {
            var key = _cacheKeyGenerator.Generate(args)+"stopwatch";

            if (args.Parameters.ContainsKey(key))
            {
                Stopwatch stopwatch = args.Parameters[key] as Stopwatch;
                args.Parameters.Remove(key);

                if (stopwatch != null)
                {
                    stopwatch.Stop();
                    if (stopwatch.ElapsedMilliseconds > _debugSettings.SlowModelThreshold)
                    {
                        var finaltType = args.Result.GetType();

                       _log.Warn("Slow Glass Model - Time: {0} Cachable: {1} Type: {2} Key: {3}".Formatted(stopwatch.ElapsedMilliseconds, args.Configuration.Cachable, finaltType.FullName , key));
                    }
                }
            }
        }
    }
}
