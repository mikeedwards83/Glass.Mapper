using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;

namespace Glass.Mapper.Umb
{
    public class UmbracoContext : UmbracoService, IUmbracoContext
    {
        public UmbracoContext(string contextName = Context.DefaultContextName)
            : base(ApplicationContext.Current.Services.ContentService, contextName)
        {
            
        }

        public T GetCurrentPage<T>() where T:class
        {
            return this.GetItem<T>(Umbraco.Web.UmbracoContext.Current.PageId);
        }
    }
}
