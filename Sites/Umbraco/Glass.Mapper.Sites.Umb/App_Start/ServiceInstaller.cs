using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Glass.Mapper.Sites.Umb.App_Start
{
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
          //  container.Register(
          //          Component.For<IRssService>().ImplementedBy<RssService>().LifestyleTransient()
          //      );
        }
    }
}