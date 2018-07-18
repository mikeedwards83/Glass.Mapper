using System;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sc
{
    /// <summary>
    /// Class ItemEditing
    /// </summary>
    public class ItemEditing : IDisposable
    {
        private readonly Item _item;
        private SecurityDisabler _securityDisabler;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemEditing"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="disableSecurity">if set to <c>true</c> [disable security].</param>
        public ItemEditing(Item item, bool disableSecurity)
        {
            _item = item;
            if (disableSecurity)
                _securityDisabler = new SecurityDisabler();

            _item.Editing.BeginEdit();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
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




