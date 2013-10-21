using Castle.DynamicProxy;
using Glass.Mapper;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Configuration;
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
namespace Glass.Mapper.Sc.V7.ContentSearch.DocumentTypeMapper
{
   public class GlassLuceneDocumentTypeMapper : DefaultLuceneDocumentTypeMapper
  {
    private SitecoreContext _sitecoreContext;

    public GlassLuceneDocumentTypeMapper():base()
    {
      
    }

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
      SearchInterceptor searchInterceptor = Enumerable.FirstOrDefault<IInterceptor>((IEnumerable<IInterceptor>) target.GetInterceptors(), (Func<IInterceptor, bool>) (x => x is SearchInterceptor)) as SearchInterceptor;
      if (searchInterceptor == null)
        return;
      AbstractTypeConfiguration typeConfiguration1 = this._sitecoreContext.GlassContext.GetTypeConfiguration(target);
      SitecoreTypeConfiguration typeConfiguration2 = new SitecoreTypeConfiguration();
      searchInterceptor.TypeConfiguration = typeConfiguration2;
      SitecoreIdConfiguration sitecoreIdConfiguration = Enumerable.FirstOrDefault<AbstractPropertyConfiguration>(typeConfiguration1.Properties, (Func<AbstractPropertyConfiguration, bool>) (x => x is SitecoreIdConfiguration)) as SitecoreIdConfiguration;
      if (sitecoreIdConfiguration == null)
        return;
      if (sitecoreIdConfiguration.PropertyInfo.PropertyType == typeof (Guid))
      {
        Guid id = (Guid) sitecoreIdConfiguration.PropertyInfo.GetValue((object) target);
        searchInterceptor.Id = new ID(id);
      }
      else if (sitecoreIdConfiguration.PropertyInfo.PropertyType == typeof (ID))
        searchInterceptor.Id = (ID) sitecoreIdConfiguration.PropertyInfo.GetValue((object) target);
      foreach (AbstractPropertyConfiguration property in typeConfiguration1.Properties)
      {
        string propName = property.PropertyInfo.Name.ToLower();
        if (!Enumerable.Any<string>(fieldNames, (Func<string, bool>) (x => x == propName)) && !Enumerable.Any<object>((IEnumerable<object>) property.PropertyInfo.GetCustomAttributes(true), (Func<object, bool>) (x => x is IIndexFieldNameFormatterAttribute)) && !(property is SitecoreIdConfiguration))
          typeConfiguration2.AddProperty(property);
      }
      typeConfiguration2.Type = typeConfiguration1.Type;
      typeConfiguration2.IdConfig = sitecoreIdConfiguration;
    }
  }
}
