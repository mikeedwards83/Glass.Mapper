using System.Diagnostics;
using Glass.Mapper.Caching;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.Diagnostics
{
    public class ConstructionTimerStart : AbstractObjectConstructionTask
    {
        private ICacheKeyGenerator _cacheKeyGenerator;
        private readonly ILog _log;
        private readonly Config.DebugSettings _debugSettings;

        public ConstructionTimerStart(
            ICacheKeyGenerator cacheKeyGenerator, 
            ILog log,
               Config.DebugSettings debugSettings)
        {
            _cacheKeyGenerator = cacheKeyGenerator;
            _log = log;
            _debugSettings = debugSettings;
            Name = "ConstructionTimerStart";
        }

        public override void Execute(ObjectConstructionArgs args)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            try
            {
                base.Execute(args);
            }
            finally
            {
                stopwatch.Stop();
                if (stopwatch.ElapsedMilliseconds > _debugSettings.SlowModelThreshold)
                {

                    var key = _cacheKeyGenerator.Generate(args) + "stopwatch";
                    var finaltType = args.Result.GetType();

                    _log.Warn("Slow Glass Model - Time: {0} Cachable: {1} Type: {2} Key: {3}".Formatted(stopwatch.ElapsedMilliseconds, args.Options.Cache.IsEnabled(), finaltType.FullName, key));
                }
            }
        }
    }
}
