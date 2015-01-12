using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Glass.Mapper
{
    public class DisableCache : IDisposable
    {
        private static CacheSetting _cacheSetting;
        private  const string ItemsKey = "D4F757BB-B53C-4600-8264-AA94D7D5050A";
        private bool _isDisposed;

        public DisableCache()
        {
            Current = CacheSetting.Disabled;
        }

        public static CacheSetting Current
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return _cacheSetting;
                    
                }
                if (HttpContext.Current.Items[ItemsKey] != null)
                {
                    return (CacheSetting) HttpContext.Current.Items[ItemsKey];
                }
                return CacheSetting.Enabled;
                
            }
            set
            {
                if (HttpContext.Current == null)
                {
                    _cacheSetting = value;

                }
                if (HttpContext.Current.Items[ItemsKey] != null)
                {
                    HttpContext.Current.Items[ItemsKey] = value;
                }
            }
        }


        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;
            Current = CacheSetting.Enabled;
        }

    }

    public enum CacheSetting
    {
        Enabled,
        Disabled
    }
}
