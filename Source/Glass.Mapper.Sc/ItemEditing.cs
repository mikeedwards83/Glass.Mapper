using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sc
{
    public class ItemEditing : IDisposable
    {
        private readonly Item _item;
        private SecurityDisabler _securityDisabler;
        private bool _disposed;

        public ItemEditing(Item item, bool disableSecurity)
        {
            _item = item;
            if (disableSecurity)
                _securityDisabler = new SecurityDisabler();

            _item.Editing.BeginEdit();
        }

        public void Dispose()
        {
            if (_disposed) return;

            _item.Editing.EndEdit();

            if(_securityDisabler != null)
                _securityDisabler.Dispose();

            _disposed = true;
        }
    }
}
