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
        public DisableCache() : base(CacheSetting.Disabled)
        {
        }
    }

    public enum CacheSetting
    {
        Enabled,
        Disabled
    }
}
