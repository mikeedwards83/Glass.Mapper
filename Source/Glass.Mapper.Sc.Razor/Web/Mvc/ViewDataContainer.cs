using System.Web.Mvc;

namespace Glass.Mapper.Sc.Razor.Web.Mvc
{
    /// <summary>
    /// Class ViewDataContainer
    /// </summary>
    public class ViewDataContainer : IViewDataContainer
    {
        /// <summary>
        /// Gets or sets the view data dictionary.
        /// </summary>
        /// <value>The view data.</value>
        /// <returns>The view data dictionary.</returns>
        public ViewDataDictionary ViewData
        {
            get;
            set;
        }
    }
}
