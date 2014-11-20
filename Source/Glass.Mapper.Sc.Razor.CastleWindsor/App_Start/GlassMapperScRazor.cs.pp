/*************************************

DO NOT CHANGE THIS FILE - UPDATE GlassMapperScCustom.cs

**************************************/

using System;
using System.Linq;
using Glass.Mapper.Sc.CastleWindsor;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Razor;
using Glass.Mapper.Configuration.Attributes;

// WebActivator has been removed. If you wish to continue using WebActivator uncomment the line below
// and delete the Glass.Mapper.Sc.CastleWindsor.config file from the Sitecore Config Include folder.
// [assembly: WebActivatorEx.PostApplicationStartMethod(typeof($rootnamespace$.App_Start.GlassMapperScRazor), "Start")]

namespace $rootnamespace$.App_Start
{
	public static class  GlassMapperScRazor
	{
		public static void Start()
		{
			//create config
			var config = new Config();

			//create the resolver
			var resolver = DependencyResolver.CreateStandardResolver();

			resolver.Container.Install(new SitecoreInstaller(config));

			//create a context
			var context = Glass.Mapper.Context.Create(resolver, GlassRazorSettings.ContextName);

			var loader = new AttributeConfigurationLoader("Glass.Mapper.Sc.Razor");

			context.Load(       
				loader       				
				);

			
		}
	}
}