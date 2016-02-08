using Castle.DynamicProxy;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.ContentSearch.Pipelines.ObjectConstruction.Tasks.SearchProxy;
using Lucene.Net.Documents;
using Sitecore.ContentSearch.Linq.Common;
using Sitecore.ContentSearch.Linq.Methods;
using Sitecore.ContentSearch.LuceneProvider;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.ContentSearch.Security;
using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Glass.Mapper.Sc.ContentSearch.LuceneProvider
{
    public class GlassLuceneDocumentTypeMapper : DefaultLuceneDocumentTypeMapper
    {
        private SitecoreContext _sitecoreContext;

        public override TElement MapToType<TElement>(Document document, SelectMethod selectMethod, IEnumerable<IFieldQueryTranslator> virtualFieldProcessors, SearchSecurityOptions securityOptions)
        {
            if (typeof (TElement) == typeof (SitecoreUISearchResultItem))
                return base.MapToType<TElement>(document, selectMethod, virtualFieldProcessors, securityOptions);

            _sitecoreContext = new SitecoreContext();
            if (selectMethod != null)
            {
                var parameters = selectMethod.Delegate.Method.GetParameters();
                if (parameters.Length != 2) throw new InvalidOperationException("Invalid number of select delegate parameters.");
                var underlyingSystemType = parameters[1].ParameterType.UnderlyingSystemType;
                var instance = Activator.CreateInstance(underlyingSystemType);
                var strArray = selectMethod.FieldNames == null || selectMethod.FieldNames.Length <= 0 ? null : selectMethod.FieldNames;

                ReadDocumentFields(document, strArray, GetTypeMap(underlyingSystemType), virtualFieldProcessors, instance);
                return (TElement)selectMethod.Delegate.DynamicInvoke(instance);
            }
            else
            {
                var documentFieldNames = GetDocumentFieldNames(document);
                var templateId = document.Get("_template");
                var instance = CreateInstance<TElement>(templateId);
                var typeMap = GetTypeMap(instance.GetType());

                //TODO: use ID.Parse on document.Get("_id") ?
                Guid id;
                if (Guid.TryParse(document.Get("_group"), out id)) SetupProxy(id, (object)instance as IProxyTargetAccessor);

                ReadDocumentFields(document, documentFieldNames, typeMap, virtualFieldProcessors, instance);

                return instance;
            }
        }

        protected virtual T CreateInstance<T>(string templateId)
        {
            var typeCreationContext = new SitecoreTypeCreationContext
                {
                    SitecoreService = _sitecoreContext,
                    RequestedType = typeof(T),
                    InferType = true,
                    IsLazy = true,
                    TemplateId = new ID(templateId)
                };
            using (new SearchSwitcher()) return (T)_sitecoreContext.InstantiateObject(typeCreationContext);
        }

        protected void SetupProxy(Guid id, IProxyTargetAccessor target)
        {
            var searchInterceptor = target.GetInterceptors().FirstOrDefault(x => x is SearchInterceptor) as SearchInterceptor;
            if (searchInterceptor == null) return;

            searchInterceptor.Id = new ID(id);
            searchInterceptor.TypeConfiguration = _sitecoreContext.GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>(target);
        }
    }
}
