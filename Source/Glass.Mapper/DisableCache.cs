using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Glass.Mapper
{
    public class DisableCache :SettingStack<CacheSetting>
    {
        const string Key = "AADBFF1C-2EE9-475A-B5CA-3D0F32A1CECC";

        public DisableCache() : base(CacheSetting.Disabled, Key)
        {
        }

        public static CacheSetting Current
        {
            get { return GetCurrent(Key); }
        }
    }

    public enum CacheSetting
    {
        Enabled,
        Disabled
    }
}
