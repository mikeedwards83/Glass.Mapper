using Glass.Mapper.IoC;
using Glass.Mapper.Sc.DataMappers;
using Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters;

namespace Glass.Mapper.Sc.IoC
{
    public class DataMapperConfigFactory : AbstractConfigFactory<AbstractDataMapper>
    {
        private readonly IConfigFactory<ISitecoreQueryParameter> queryParameterFactory;

        public DataMapperConfigFactory(IConfigFactory<ISitecoreQueryParameter> queryParameterFactory)
        {
            this.queryParameterFactory = queryParameterFactory;
            Init();
        }

        protected void Init()
        {
            Add(() => new SitecoreLazyMapper());
            Add(() => new SitecoreIgnoreMapper());
            Add(() => new SitecoreChildrenCastMapper());
            Add(() => new SitecoreChildrenMapper());
            Add(() => new SitecoreFieldBooleanMapper());
            Add(() => new SitecoreFieldDateTimeMapper());
            Add(() => new SitecoreFieldDecimalMapper());
            Add(() => new SitecoreFieldDoubleMapper());
            Add(() => new SitecoreFieldEnumMapper());
            Add(() => new SitecoreFieldFileMapper());
            Add(() => new SitecoreFieldFloatMapper());
            Add(() => new SitecoreFieldGuidMapper());
            Add(() => new SitecoreFieldHtmlEncodingMapper());
            Add(() => new SitecoreFieldIEnumerableMapper());
            Add(() => new SitecoreFieldImageMapper());
            Add(() => new SitecoreFieldIntegerMapper());
            Add(() => new SitecoreFieldLinkMapper());
            Add(() => new SitecoreFieldLongMapper());
            Add(() => new SitecoreFieldNameValueCollectionMapper());
            Add(() => new SitecoreFieldDictionaryMapper());
            Add(() => new SitecoreFieldNullableDateTimeMapper());
            Add(() => new SitecoreFieldNullableDoubleMapper());
            Add(() => new SitecoreFieldNullableDecimalMapper());
            Add(() => new SitecoreFieldNullableFloatMapper());
            Add(() => new SitecoreFieldNullableGuidMapper());
            Add(() => new SitecoreFieldNullableIntMapper());
            Add(() => new SitecoreFieldNullableEnumMapper());
            Add(() => new SitecoreFieldRulesMapper());
            Add(() => new SitecoreFieldStreamMapper());
            Add(() => new SitecoreFieldStringMapper());
            Add(() => new SitecoreFieldTypeMapper());
            Add(() => new SitecoreIdMapper());
            Add(() => new SitecoreItemMapper());
            Add(() => new SitecoreInfoMapper());
            Add(() => new SitecoreNodeMapper());
            Add(() => new SitecoreLinkedMapper());
            Add(() => new SitecoreParentMapper());
            Add(() => new SitecoreDelegateMapper());
            Add(() => new SitecoreQueryMapper(queryParameterFactory.GetItems()));
        }
    }
}
