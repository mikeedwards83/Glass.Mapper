using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data.Serialization;

namespace Glass.Mapper.Sc
{
    /// <summary>
    /// Disables the itemhandler serializations
    /// </summary>
    public class DisableItemHandler: IDisposable
    {
        private bool _wasDisabled;

        public DisableItemHandler()
        {
            _wasDisabled = ItemHandler.DisabledLocally;
            ItemHandler.DisabledLocally = true;
        }

        public void Dispose()
        {
            ItemHandler.DisabledLocally = _wasDisabled;
        }
    }
}
