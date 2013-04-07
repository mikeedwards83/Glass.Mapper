using System;
using Glass.Mapper.Sc.CastleWindsor;
using Glass.Mapper.Sc.Configuration.Attributes;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof($rootnamespace$.App_Start.GlassMapperSc), "Start")]

namespace $rootnamespace$.App_Start
{
	public static class  GlassMapperSc
	{
		public static void Start()
		{
			var config = GlassMapperScCustom.GetConfig();

			//create the resolver
			var resolver = DependencyResolver.CreateStandardResolver(config);

			//install the custom services
			var container = (resolver as DependencyResolver).Container;
			GlassMapperScCustom.CastleConfig(container);

			//create a context
			var context = Glass.Mapper.Context.Create(resolver);

			var attributes = new SitecoreAttributeConfigurationLoader("$assemblyname$");

			var loaders = GlassMapperScCustom.GlassLoaders();

			context.Load(              
				loaders.Union(new []{attributes});
				);
		}
	}
}