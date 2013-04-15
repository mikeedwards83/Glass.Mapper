/*************************************

DO NOT CHANGE THIS FILE - UPDATE GlassMapperScCustom.cs

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
			//create the resolver
			var resolver = DependencyResolver.CreateStandardResolver();

			//install the custom services
			GlassMapperScCustom.CastleConfig(resolver.Container);

			//create a context
			var context = Glass.Mapper.Context.Create(resolver);
			context.Load(      
				GlassMapperScCustom.GlassLoaders()        				
				);

			GlassMapperScCustom.PostLoad();
		}
	}
}