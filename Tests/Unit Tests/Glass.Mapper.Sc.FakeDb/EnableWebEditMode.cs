using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper.Sc.FakeDb
{

    /// <summary>
    /// Enabling Web Edit mode involves jumping through a load of hoops. This hopes to simplify some of that
    /// This is not thread safe.
    /// </summary>
    public class EnableWebEditMode : IDisposable
    {
        public static bool AllowWebEdit { get; private set; }
        private Func<bool> _originalFunc;


        public EnableWebEditMode()
        {
            _originalFunc = Sc.Utilities.GetIsPageEditorEditing;
            Sc.Utilities.GetIsPageEditorEditing = () => true;
            AllowWebEdit = true;
        }
        public void Dispose()
        {
            Sc.Utilities.GetIsPageEditorEditing = _originalFunc;
            AllowWebEdit = false;

        }
    }
}
