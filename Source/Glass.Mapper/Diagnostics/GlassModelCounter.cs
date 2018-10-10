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

        public int _requestedCountBefore = 0;
        public int _cacheCount = 0;
        public bool _enabled;
        public ModelCounter Counter { get; private set; }

        public int RequestCount { get; private set; }
        public int CacheCount { get; private set; }

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
            Counter = ModelCounter.Instance;

            RequestCount = -1;
            CacheCount = -1;

            if (_enabled)
            {
                _requestedCountBefore = Counter.ModelsRequested;
                _cacheCount = Counter.CachedModels;
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
                int endRequestCount = Counter.ModelsRequested;
                int endCacheCount = Counter.CachedModels;

                int diffRequest = endRequestCount - _requestedCountBefore;
                int diffCache = endCacheCount - _cacheCount;

                RequestCount = diffRequest;
                CacheCount = diffCache;

                if (diffRequest > _threshold || diffCache > _threshold)
                {
                    _log.Debug("Glass Counter {0} - Requests: {1} Cache Hits: {2}".Formatted(_identifier, diffRequest,
                        diffCache));
                }
            }
        }
    }
}
