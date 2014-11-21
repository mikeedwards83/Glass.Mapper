/*************************************

DO NOT CHANGE THIS FILE - UPDATE GlassMapperScCustom.cs

**************************************/

using System;
using System.Linq;
using Glass.Mapper.Sc.CastleWindsor;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.Pipelines;

// WebActivator has been removed. If you wish to continue using WebActivator uncomment the line below
// and delete the Glass.Mapper.Sc.CastleWindsor.config file from the Sitecore Config Include folder.
// [assembly: WebActivatorEx.PostApplicationStartMethod(typeof($rootnamespace$.App_Start.GlassMapperSc), "Start")]

namespace $rootnamespace$.App_Start
{
	public class  GlassMapperSc
	{
		public void Process(PipelineArgs args){
			GlassMapperSc.Start();
		}

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