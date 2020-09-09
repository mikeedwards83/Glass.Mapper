using System.Collections.Specialized;
using Sitecore.Data;
using Sitecore.Data.Events;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sc
{
    public class RenderingParametersModelFactory
    {
        private readonly ISitecoreService _sitecoreService;

        public RenderingParametersModelFactory(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }

        public virtual T CreateModel<T>(NameValueCollection parameters, ID renderParametersTemplateId) where T:class
        {
            var item = Utilities.CreateFakeItem(null, renderParametersTemplateId, _sitecoreService.Database, "renderingParameters");

            using (new SecurityDisabler())
            {
                using (new EventDisabler())
                {
                    if (parameters != null)
                    {
                        item.Editing.BeginEdit();
                        item.RuntimeSettings.Temporary = true;
                        foreach (var key in parameters.AllKeys)
                        {
                            var fld = item.Fields[key];
                            if (fld != null)
                            {
                                fld.SetValue(parameters[key], true);
                            }
                        }
                    }

                    var options = new GetItemByItemOptions
                    {
                        Item = item,
                        //this must be only reference properties to force the fake item to be read before it is deleted
                        //it isn't possible lazy load the top level
                        Lazy = LazyLoading.OnlyReferenced,
                        VersionCount = false
                    };
                    T obj = _sitecoreService.GetItem<T>(options);

                    item.Editing.CancelEdit();

                    if (_sitecoreService.Config.DeleteRenderingParameterItems)
                    {
                        item.Delete(); //added for clean up
                    }

                    return obj;
                }
            }
        }
    }
}
