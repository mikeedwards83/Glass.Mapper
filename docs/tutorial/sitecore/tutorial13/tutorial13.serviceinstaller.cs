using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Glass.Mapper.Sites.Sc.Services.BbcNews;

namespace Glass.Mapper.Sites.Sc.App_Start
{
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                    Component.For<IRssService>().ImplementedBy<RssService>().LifestyleTransient()
                );
        }
    }
}