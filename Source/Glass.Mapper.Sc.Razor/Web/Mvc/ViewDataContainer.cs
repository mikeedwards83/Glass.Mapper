using System.Web.Mvc;

namespace Glass.Mapper.Sc.Razor.Web.Mvc
{
    public class ViewDataContainer : IViewDataContainer
    {
        public ViewDataDictionary ViewData
        {
            get;
            set;
        }
    }
}
