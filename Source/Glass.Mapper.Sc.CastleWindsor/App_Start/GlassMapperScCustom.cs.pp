using System.Collections.Generic;
using Castle.Windsor;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.CastleWindsor;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace $rootnamespace$.App_Start
{
    public static  class GlassMapperScCustom
    {
		public static Config GetConfig(){
			return new Config();
		}
		public static void CastleConfig(IWindsorContainer container, Config config){
			container.Install(new SitecoreInstaller(config));
		}
		public static IConfigurationLoader[] GlassLoaders(){
			var attributes = new SitecoreAttributeConfigurationLoader("$assemblyname$");
			
			return new IConfigurationLoader[]{attributes};
		}
    }
}
