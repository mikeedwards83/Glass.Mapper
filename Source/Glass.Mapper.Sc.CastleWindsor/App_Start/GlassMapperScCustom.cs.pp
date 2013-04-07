using System.Collections.Generic;
using Castle.Windsor;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.CastleWindsor;

namespace $rootnamespace$.App_Start
{
    public static  class GlassMapperScCustom
    {
		public static Config GetConfig(){
			return new Config();
		}
		public static void CastleConfig(IWindsorContainer container){

		}
		public static IEnumerable<IConfigurationLoader> GlassLoaders(){
			return new IConfigurationLoader[]{};
		}
    }
}
