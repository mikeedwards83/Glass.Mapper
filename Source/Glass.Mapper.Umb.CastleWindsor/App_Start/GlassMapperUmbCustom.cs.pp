using System.Collections.Generic;
using Castle.Windsor;
using Glass.Mapper.Configuration;
using Glass.Mapper.Umb.CastleWindsor;
using Glass.Mapper.Umb.Configuration.Attributes;

namespace $rootnamespace$.App_Start
{
    public static class GlassMapperUmbCustom
    {
		public static Config GetConfig(){
			return new Config();
		}
		public static void CastleConfig(IWindsorContainer container, Config config){
			container.Install(new UmbracoInstaller(config));
		}
		public static IConfigurationLoader[] GlassLoaders(){
			var attributes = new UmbracoAttributeConfigurationLoader("$assemblyname$");
			
			return new IConfigurationLoader[]{attributes};
		}
    }
}
