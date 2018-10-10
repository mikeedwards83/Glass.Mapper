using System.Collections.Generic;
using System.Linq;
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
		public virtual void Process(PipelineArgs args)
		{
			Start();
		}

		public virtual void Start()
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

		public virtual IDependencyResolver CreateResolver()
		{
		    return CreateResolver(new DependencyResolver(new Config()));
		}

	    protected virtual IDependencyResolver CreateResolver(IDependencyResolver resolver)
	    {
	        var createResolverArgs = new CreateResolverPipelineArgs
	        {
	            DependencyResolver = resolver
            };
	        CreateResolverPipeline.Run(createResolverArgs);
	        return createResolverArgs.DependencyResolver;
        }


		public virtual IConfigurationLoader[] GetGlassLoaders()
		{
			var getGlassLoadersArgs = new GetGlassLoadersPipelineArgs
			{
				GlassLoaders = new List<IConfigurationLoader>()
			};
			GetGlassLoadersPipeline.Run(getGlassLoadersArgs);
			return getGlassLoadersArgs.GlassLoaders.ToArray();

            
		}

		public virtual void LoadConfigurationMaps(IDependencyResolver resolver, Glass.Mapper.Context context)
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

		public virtual void AddMaps(IConfigFactory<IGlassMap> mapsConfigFactory)
		{
			var pipelineArgs = new AddMapsPipelineArgs { MapsConfigFactory = mapsConfigFactory };
			AddMapsPipeline.Run(pipelineArgs);
		}

		public virtual void PostLoad(IDependencyResolver dependencyResolver)
		{
			var postLoadArgs = new PostLoadPipelineArgs
			{
				DependencyResolver = dependencyResolver
			};
			PostLoadPipeline.Run(postLoadArgs);
		}
	}
}
