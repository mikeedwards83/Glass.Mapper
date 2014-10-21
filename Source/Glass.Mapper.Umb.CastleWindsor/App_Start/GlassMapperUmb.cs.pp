using System;
using Glass.Mapper.Umb.CastleWindsor;
using Glass.Mapper.Umb.Configuration.Attributes;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof($rootnamespace$.App_Start.GlassMapperUmb), "Start")]

namespace $rootnamespace$.App_Start
{
    public static class  GlassMapperUmb
    {
        public static void Start()
        {
            //create the resolver
            var resolver = DependencyResolver.CreateStandardResolver();

			//install the custom services
			GlassMapperUmbCustom.CastleConfig(resolver.Container);

            //create a context
            var context = Glass.Mapper.Context.Create(resolver);
            context.Load(    
				GlassMapperUmbCustom.GlassLoaders()   
                );
        }
    }
}
