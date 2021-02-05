using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Configuration;
using Glass.Mapper.Maps;
using Glass.Mapper.Sc.Configuration.Fluent;
using Glass.Mapper.Sc.IoC;
using Glass.Mapper.Sc.Pipelines.GetGlassLoaders;
using Glass.Mapper.Sc.Pipelines.PostLoad;
using Glass.Mapper.Sc.Web;
using Microsoft.Extensions.DependencyInjection;

namespace Glass.Mapper.Sc
{
    public static class ServiceCollectionExtensionMethods
    {

        

        public static IGlassServiceConfiguration AddGlassMapper(this IServiceCollection collection,  Action<GlassMapperScOptions> config = null)
        {
            var glassConfig = new GlassServiceConfiguration();
            glassConfig.ServiceCollection = collection;

            var options = new GlassMapperScOptions();
            if (config != null)
            {
                config(options);
            }

            glassConfig.ContextName = options.ContextName;

            var resolver = new DependencyResolver(options.Config);

            options.ConfigureResolver(resolver);

            var context = Context.Create(resolver, options.ContextName, true);

            var dependencyResolver = resolver as DependencyResolver;
            if (dependencyResolver != null)
            {
                if (dependencyResolver.ConfigurationMapFactory is ConfigurationMapConfigFactory)
                {
                    foreach (var map in options.GlassMaps)
                    {
                        dependencyResolver.ConfigurationMapFactory.Add(map);
                    }
                }

                IConfigurationMap configurationMap = new ConfigurationMap(dependencyResolver);
                SitecoreFluentConfigurationLoader configurationLoader =
                    configurationMap.GetConfigurationLoader<SitecoreFluentConfigurationLoader>();
                context.Load(configurationLoader);
            }

            var getGlassLoadersArgs = new GetGlassLoadersPipelineArgs
            {
                Context = context,
                Loaders = new List<IConfigurationLoader>()
            };
            GetGlassLoadersPipeline.Run(getGlassLoadersArgs);

            context.Load(options.Loaders.ToArray());
            context.Load(getGlassLoadersArgs.Loaders.ToArray());

            options.PostLoad();

            var postLoadArgs = new PostLoadPipelineArgs
            {
                DependencyResolver = dependencyResolver
            };

            PostLoadPipeline.Run(postLoadArgs);


            return glassConfig;

        }

  

    }


}
