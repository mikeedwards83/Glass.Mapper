/*************************************

DO NTO CHANGE THIS FILE - UPDATE GlassMapperScCustom.cs

**************************************/




using System;
using System.Linq;
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
			GlassMapperScCustom.CastleConfig(container, config);

			//create a context
			var context = Glass.Mapper.Context.Create(resolver);
			context.Load(      
				GlassMapperScCustom.GlassLoaders()        				
				);
		}
	}
}