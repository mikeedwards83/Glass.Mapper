using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.TypeResolver;
using Castle.Windsor;

namespace Glass.Mapper
{
    public abstract class GlassCastleConfigBase : IGlassConfiguration
    {
        public virtual void Setup(WindsorContainer container, string contextName)
        {
            var typeTasks = TypeResolverTasks(contextName);
            var configTasks = ConfigurationResolverTasks(contextName);
            var objectTasks = ObjectContructionTasks(contextName);

            container.Register(typeTasks.ToArray());
            container.Register(configTasks.ToArray());
            container.Register(objectTasks.ToArray());

            container.Register(
                Component.For<ObjectFactory>().LifestyleTransient().DynamicParameters(
                    (k, d) =>
                        {
                            var types = k.ResolveAll<ITypeResolverTask>();
                            var configs = k.ResolveAll<IConfigurationResolverTask>();
                            var objs = k.ResolveAll<IObjectConstructionTask>();

                            d.Add("typeResolverTasks", types);
                            d.Add("configurationResolverTasks", configs);
                            d.Add("objectConstructionTasks", objs);
                        })
                );
        }

        public abstract IEnumerable<ComponentRegistration<IObjectConstructionTask>> ObjectContructionTasks(string contextName);
        public abstract IEnumerable<ComponentRegistration<ITypeResolverTask>> TypeResolverTasks(string contextName);
        public abstract IEnumerable<ComponentRegistration<IConfigurationResolverTask>> ConfigurationResolverTasks(string contextName);
       
    }
}
