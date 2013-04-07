using Castle.Windsor;

namespace $rootnamespace$.App_Start
{
    public static  class GlassMapperScCustom
    {
		public static void CastleConfig(IWindsorContainer container){

		}
		public static IEnumerable<IConfigurationLoader> GlassLoaders(){
			return new IConfigurationLoader[]{};
		}
    }
}
