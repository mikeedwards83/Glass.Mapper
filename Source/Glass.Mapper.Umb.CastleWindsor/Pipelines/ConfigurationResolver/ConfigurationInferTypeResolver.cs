using System.Linq;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Umb.Configuration;

namespace Glass.Mapper.Umb.CastleWindsor.Pipelines.ConfigurationResolver
{
    public class ConfigurationInferTypeResolver : IConfigurationResolverTask
    {
        public void Execute(ConfigurationResolverArgs args)
        {
            if (args.AbstractTypeCreationContext.InferType)
            {
                var umbContext = args.AbstractTypeCreationContext as UmbracoTypeCreationContext;
                if (umbContext != null)
                {
                    var content = umbContext.Content;

                    var configs = args.Context.TypeConfigurations.Select(config => config.Value as UmbracoTypeConfiguration);
                    var types = configs.Where(x => x.ContentTypeAlias == content.ContentType.Alias);

                    if (!types.Any()) return;
                    args.Result = types.FirstOrDefault();
                }
            }
        }
    }
}
