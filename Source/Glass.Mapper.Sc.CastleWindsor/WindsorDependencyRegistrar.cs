using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Glass.Mapper.Sc.CastleWindsor
{
    public class WindsorDependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// The container
        /// </summary>
        protected IWindsorContainer Container { get; private set; }

        public WindsorDependencyRegistrar(IWindsorContainer container)
        {
            Container = container;
        }

        /// <summary>
        /// Registers an item in the container transiently
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TComponent"></typeparam>
        public void RegisterTransient<T, TComponent>() where T : class
        {
            Container.Register(Component.For<T, TComponent>().LifestyleCustom<NoTrackLifestyleManager>());
        }

        /// <summary>
        /// Registers an instance of an object
        /// </summary>
        /// <param name="instance">The instance</param>
        /// <typeparam name="T"></typeparam>
        public void RegisterInstance<T>(T instance) where T : class
        {
            Container.Register(Component.For<T>().Instance(instance));
        }
    }
}
