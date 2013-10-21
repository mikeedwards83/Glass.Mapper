using Castle.DynamicProxy;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.ContentSearch.Pipelines.ObjectConstruction.Tasks.SearchProxy;
using Lucene.Net.Documents;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq.Common;
using Sitecore.ContentSearch.Linq.Methods;
using Sitecore.ContentSearch.LuceneProvider;
using Sitecore.ContentSearch.Security;
using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Glass.Mapper.Sc.ContentSearch.LuceneProvider
{
   public class GlassLuceneDocumentTypeMapper : DefaultLuceneDocumentTypeMapper
  {
    private SitecoreContext _sitecoreContext;

    public virtual TElement MapToType<TElement>(Document document, SelectMethod selectMethod, IEnumerable<IFieldQueryTranslator> virtualFieldProcessors, SearchSecurityOptions securityOptions)
    {
      if (selectMethod != null)
      {
        ParameterInfo[] parameters = selectMethod.Delegate.Method.GetParameters();
        if (parameters.Length != 2)
          throw new InvalidOperationException("Invalid number of select delegate parameters.");
        Type underlyingSystemType = parameters[1].ParameterType.UnderlyingSystemType;

        var instance = Activator.CreateInstance(underlyingSystemType);
        string[] strArray = selectMethod.FieldNames  == null || selectMethod.FieldNames.Length <= 0 ? (string[]) null : selectMethod.FieldNames;
        ReadDocumentFields<object>(document, (IEnumerable<string>) strArray, GetTypeMap(underlyingSystemType), virtualFieldProcessors, instance);
        return (TElement)selectMethod.Delegate.DynamicInvoke(instance);
      }
      else
      {
        DocumentTypeMapInfo typeMap = GetTypeMap<TElement>();
        IEnumerable<string> documentFieldNames = GetDocumentFieldNames(document);
        TElement instance = this.CreateInstance<TElement>();
        ReadDocumentFields<TElement>(document, documentFieldNames, typeMap, virtualFieldProcessors, instance);

        this.SetupProxy<TElement>(documentFieldNames ?? Enumerable.Select<IFieldable, string>((IEnumerable<IFieldable>) document.GetFields(), (Func<IFieldable, string>) (x => x.Name.ToLower())), (object) instance as IProxyTargetAccessor);
        return instance;
      }
    }

    protected virtual T CreateInstance<T>()
    {
      SitecoreTypeCreationContext typeCreationContext = new SitecoreTypeCreationContext();
        typeCreationContext.RequestedType = new[] {typeof (T)};
      typeCreationContext.IsLazy = true;
      typeCreationContext.SitecoreService = (SitecoreService) this._sitecoreContext;
      using (new SearchSwitcher())
        return (T) this._sitecoreContext.InstantiateObject((AbstractTypeCreationContext) typeCreationContext);
    }

       protected void SetupProxy<T>(IEnumerable<string> fieldNames, IProxyTargetAccessor target)
       {
           SearchInterceptor searchInterceptor =
               Enumerable.FirstOrDefault<IInterceptor>((IEnumerable<IInterceptor>) target.GetInterceptors(),
                                                       (Func<IInterceptor, bool>) (x => x is SearchInterceptor)) as
               SearchInterceptor;
           if (searchInterceptor == null)
               return;

           searchInterceptor.TypeConfiguration =
               (SitecoreTypeConfiguration) _sitecoreContext.GlassContext.GetTypeConfiguration(target);
           SitecoreIdConfiguration sitecoreIdConfiguration = searchInterceptor.TypeConfiguration.IdConfig;
           if (sitecoreIdConfiguration == null)
               return;

           if (sitecoreIdConfiguration.PropertyInfo.PropertyType == typeof (ID))
           {
               searchInterceptor.Id = (ID) sitecoreIdConfiguration.PropertyInfo.GetValue(target);
           }
           if (sitecoreIdConfiguration.PropertyInfo.PropertyType == typeof (Guid))
           {
               Guid id = (Guid) sitecoreIdConfiguration.PropertyInfo.GetValue(target);
               searchInterceptor.Id = new ID(id);
           }

           searchInterceptor.IndexFields = fieldNames;

       }
  }
}
