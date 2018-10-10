#region GlassMapperSc generated code
/*************************************

DO NOT CHANGE THIS FILE - UPDATE GlassMapperScCustom.cs

**************************************/

using Glass.Mapper.Maps;
using Glass.Mapper.Sc.Configuration.Fluent;
using Glass.Mapper.Sc.IoC;
using Glass.Mapper.Sc.Pipelines.GetChromeData;
using Sitecore.Pipelines;


namespace $rootnamespace$.App_Start
{
	public class GlassMapperSc : Glass.Mapper.Sc.Pipelines.Initialize.GlassMapperSc
	{
		public override void Process(PipelineArgs args){
			Start();
		}

		public override void Start()
		{
			//install the custom services

			var resolver = CreateResolver(); 

			//create a context
			var context = Glass.Mapper.Context.Create(resolver);

			LoadConfigurationMaps(resolver, context);

			context.Load(      
				);

			GlassMapperScCustom.PostLoad();

			PostLoad();

			//EditFrameBuilder.EditFrameItemPrefix = "Glass-";

        }

        public override IDependencyResolver CreateResolver()
        {
            var resolver = GlassMapperScCustom.CreateResolver();
            base.CreateResolver(resolver);
        }
        
        public virtual IConfigurationLoader[] GetGlassLoaders(){

          var loaders1 = GlassMapperScCustom.GlassLoaders();        				
          var loaders2 = base.GetGlassLoaders();

          return loaders1.Concat(loaders2).ToArray();
        }

        public override void LoadConfigurationMaps(IDependencyResolver resolver, Glass.Mapper.Context context)
        {
            var dependencyResolver = resolver as DependencyResolver;
            if (dependencyResolver == null)
            {
                return;
            }

            if (dependencyResolver.ConfigurationMapFactory is ConfigurationMapConfigFactory)
            {
                GlassMapperScCustom.AddMaps(dependencyResolver.ConfigurationMapFactory);
            }

            IConfigurationMap configurationMap = new ConfigurationMap(dependencyResolver);
            SitecoreFluentConfigurationLoader configurationLoader = configurationMap.GetConfigurationLoader<SitecoreFluentConfigurationLoader>();
            context.Load(configurationLoader);

            base.LoadConfigurationMaps(resolver, context);
        }
	}
}
#endregion