using Glass.Mapper.IoC;
using Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters;

namespace Glass.Mapper.Sc.IoC
{
    public interface IDependencyResolver : Mapper.IoC.IDependencyResolver
    {
        IConfigFactory<ISitecoreQueryParameter> QueryParameterFactory { get; }
    }
}
