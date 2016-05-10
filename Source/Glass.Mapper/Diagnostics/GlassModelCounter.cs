using System;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.Diagnostics;

namespace Glass.Mapper.Diagnostics
{
    public class GlassModelCounter : IDisposable
    {
        private readonly ILog _log;
        private readonly string _identifier;
        private readonly int _threshold;
        private readonly string _key;

        private int _requestedCount = 0;
        private int _cacheMissCount = 0;
        private bool _enabled;

        public GlassModelCounter(IAbstractService service, string identifier, int threshold)
            :this(service.GlassContext, identifier, threshold)
        {
          
        }

        public GlassModelCounter(Glass.Mapper.Context context, string identifier, int threshold)
        {
            _log = context.DependencyResolver.GetLog();
            _identifier = identifier;
            _threshold = threshold;
            _enabled = context.Config.Debug.Enabled;

            if (_enabled)
            {
                _requestedCount = ConstructionMonitorTask.GetCounter(ConstructionMonitorTask.CalledKey);
                _cacheMissCount = ConstructionMonitorTask.GetCounter(ConstructionMonitorTask.CacheMissKey);
            }
        }

        public GlassModelCounter(IAbstractService service, string identifier)
            :this(service, identifier, 0)
        {
          
        }

        public void Dispose()
        {
            if (_enabled)
            {
                int endRequestCount = ConstructionMonitorTask.GetCounter(ConstructionMonitorTask.CalledKey);
                int endCacheMissCount = ConstructionMonitorTask.GetCounter(ConstructionMonitorTask.CacheMissKey);

                int diffRequest = endRequestCount - _requestedCount;
                int diffCache = endCacheMissCount - _cacheMissCount;

                if (diffRequest > _threshold || diffCache > _threshold)
                {
                    _log.Debug("Glass Counter {0} - Requests: {1} Cache Misses: {2}".Formatted(_identifier, diffRequest,
                        diffCache));
                }
            }
        }
    }
}
