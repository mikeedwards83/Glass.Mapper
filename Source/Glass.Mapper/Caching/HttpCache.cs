using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace Glass.Mapper.Caching
{
    public class HttpCache : ICacheManager
    {
        List<string> _keys = new List<string>();

        public int AbsoluteExpiry { get; set; }
        public int SlidingExpiry { get; set; }

        public object this[string key]
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Cache != null)
                {
                    return HttpContext.Current.Cache[key];
                }
                return null;
            }
            set { Add(key, value); }
        }

        public void ClearCache()
        {
            lock (_keys)
            {
                if (HttpContext.Current != null && HttpContext.Current.Cache != null)
                {
                    foreach (var key in _keys)
                    {
                        HttpContext.Current.Cache.Remove(key);
                    }
                }
            }
        }

        public void Add(string key, object value)
        {
            lock (_keys)
            {
                if (HttpContext.Current != null && HttpContext.Current.Cache != null)
                {
                    if (AbsoluteExpiry > 0)
                    {
                        HttpContext.Current.Cache.Add(key, value, null, DateTime.Now.AddSeconds(AbsoluteExpiry),
                            Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                    }
                    else
                    {
                        HttpContext.Current.Cache.Add(key, value, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, SlidingExpiry),
                            CacheItemPriority.Normal, null);
                    }

                    _keys.Add(key);
                }
            }
        }

        public bool Contains(string key)
        {
            if (HttpContext.Current != null && HttpContext.Current.Cache != null)
            {
                return HttpContext.Current.Cache[key] != null;
            }
            return false;
        }
    }
}
