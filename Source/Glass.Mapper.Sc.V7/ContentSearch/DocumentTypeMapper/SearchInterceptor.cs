using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper.Sc.V7.ContentSearch.DocumentTypeMapper
{
    public class SearchInterceptor : IInterceptor
    {
        private bool _isLoaded = false;
        private readonly ObjectConstructionArgs _args;
        private Dictionary<string, object> _values;

        public SitecoreTypeConfiguration TypeConfiguration { get; set; }

        public ID Id { get; set; }

        public SearchInterceptor(ObjectConstructionArgs args)
        {
            this._args = args;
            this._values = new Dictionary<string, object>();
        }

        public void Intercept(IInvocation invocation)
        {
            if (!invocation.Method.IsSpecialName || !invocation.Method.Name.StartsWith("get_") && !invocation.Method.Name.StartsWith("set_"))
                return;
            string str = invocation.Method.Name.Substring(0, 4);
            string name = invocation.Method.Name.Substring(4);
            if (!this._isLoaded && this.TypeConfiguration != null && Enumerable.Any<AbstractPropertyConfiguration>(this.TypeConfiguration.Properties, (Func<AbstractPropertyConfiguration, bool>)(x => x.PropertyInfo.Name == name)))
            {
                SitecoreTypeCreationContext typeCreationContext = this._args.AbstractTypeCreationContext as SitecoreTypeCreationContext;
                typeCreationContext.Item = typeCreationContext.SitecoreService.Database.GetItem(this.Id);
                SitecoreTypeConfiguration typeConfiguration = this.TypeConfiguration;
                AbstractDataMappingContext dataMappingContext = this._args.Service.CreateDataMappingContext(this._args.AbstractTypeCreationContext, (object)null);
                foreach (AbstractPropertyConfiguration propertyConfiguration in typeConfiguration.Properties)
                {
                    object obj = propertyConfiguration.Mapper.MapToProperty(dataMappingContext);
                    this._values[propertyConfiguration.PropertyInfo.Name] = obj;
                }
                this._isLoaded = true;
            }
            if (str == "get_")
            {
                if (this._values.ContainsKey(name))
                {
                    object obj = this._values[name];
                    invocation.ReturnValue = obj;
                }
            }
            else if (str == "set_")
                this._values[name] = invocation.Arguments[0];
            else
                throw new MapperException(Glass.Mapper.ExtensionMethods.Formatted("Method with name {0}{1} on type {2} not supported.", (object)str, (object)name, (object)this._args.Configuration.Type.FullName));
        }
    }
}
