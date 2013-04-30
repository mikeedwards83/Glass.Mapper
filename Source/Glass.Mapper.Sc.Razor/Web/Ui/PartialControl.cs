using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.Razor.Web.Ui
{
    public class PartialControl<T> : AbstractRazorControl<T>
    {
        T _tempModel;

        public void SetModel(T model)
        {
           _tempModel = model;
        }
        public override T GetModel()
        {
            return _tempModel;
        }
    }

}
