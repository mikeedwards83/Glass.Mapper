using System.Collections.Generic;
using Glass.Mapper.Configuration;
using Glass.Mapper.IoC;
using Glass.Mapper.Maps;
using Glass.Mapper.Sc.Configuration.Fluent;
using Glass.Mapper.Sc.IoC;
using Glass.Mapper.Sc.Pipelines.AddMaps;
using Glass.Mapper.Sc.Pipelines.CreateResolver;
using Glass.Mapper.Sc.Pipelines.GetGlassLoaders;
using Glass.Mapper.Sc.Pipelines.PostLoad;
using Sitecore.Pipelines;
using IDependencyResolver = Glass.Mapper.Sc.IoC.IDependencyResolver;

namespace Glass.Mapper.Sc.Pipelines.Initialize
{
	public class GlassMapperSc
	{
		public void Process(PipelineArgs args)
		{
			GlassMapperSc.Start();
		}

		public static void Start()
		{
			//install the custom services
			var resolver = CreateResolver();

			//create a context
			var context = Context.Create(resolver);

			LoadConfigurationMaps(resolver, context);

			context.Load(GetGlassLoaders());

			PostLoad(resolver);

			//EditFrameBuilder.EditFrameItemPrefix = "Glass-";
        }

		public static IDependencyResolver CreateResolver()
		{
			var createResolverArgs = new CreateResolverPipelineArgs
			{
				DependencyResolver = new DependencyResolver(new Config())
			};
			CreateResolverPipeline.Run(createResolverArgs);
			return createResolverArgs.DependencyResolver;
		}

		public static IConfigurationLoader[] GetGlassLoaders()
		{
			var getGlassLoadersArgs = new GetGlassLoadersPipelineArgs
			{
				GlassLoaders = new List<IConfigurationLoader>()
			};
			GetGlassLoadersPipeline.Run(getGlassLoadersArgs);
			return getGlassLoadersArgs.GlassLoaders.ToArray();
		}

		public static void LoadConfigurationMaps(IDependencyResolver resolver, Glass.Mapper.Context context)
        {
            var dependencyResolver = resolver as DependencyResolver;
            if (dependencyResolver == null)
            {
                return;
            }

            if (dependencyResolver.ConfigurationMapFactory is ConfigurationMapConfigFactory)
            {
                AddMaps(dependencyResolver.ConfigurationMapFactory);
            }

            IConfigurationMap configurationMap = new ConfigurationMap(dependencyResolver);
            SitecoreFluentConfigurationLoader configurationLoader = configurationMap.GetConfigurationLoader<SitecoreFluentConfigurationLoader>();
            context.Load(configurationLoader);
        }

		public static void AddMaps(IConfigFactory<IGlassMap> mapsConfigFactory)
		{
			var pipelineArgs = new AddMapsPipelineArgs { MapsConfigFactory = mapsConfigFactory };
			AddMapsPipeline.Run(pipelineArgs);
		}

		public static void PostLoad(IDependencyResolver dependencyResolver)
		{
			var postLoadArgs = new PostLoadPipelineArgs
			{
				DependencyResolver = dependencyResolver
			};
			PostLoadPipeline.Run(postLoadArgs);
		}
	}
}
