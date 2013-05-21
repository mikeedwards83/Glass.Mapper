using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.Razor.Web.Ui
{
    /// <summary>
    /// PartialControl
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PartialControl<T> : AbstractRazorControl<T>
    {
        T _tempModel;

        /// <summary>
        /// Sets the model.
        /// </summary>
        /// <param name="model">The model.</param>
        public void SetModel(T model)
        {
           _tempModel = model;
        }

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <returns></returns>
        public override T GetModel()
        {
            return _tempModel;
        }
    }

}
