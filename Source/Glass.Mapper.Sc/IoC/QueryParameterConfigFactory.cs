using Glass.Mapper.IoC;
using Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters;

namespace Glass.Mapper.Sc.IoC
{
    public class QueryParameterConfigFactory : AbstractFinalisedConfigFactory<ISitecoreQueryParameter>
    {
        public QueryParameterConfigFactory()
        {
            Init();
        }
        protected void Init()
        {
            Add(() => new ItemDateNowParameter());
            Add(() => new ItemEscapedPathParameter());
            Add(() => new ItemIdNoBracketsParameter());
            Add(() => new ItemIdParameter());
            Add(() => new ItemPathParameter());
        }
    }
}
