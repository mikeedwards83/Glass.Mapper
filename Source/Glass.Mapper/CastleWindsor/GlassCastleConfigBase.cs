using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.DataMapperResolver;
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
            var dataMapperTasks = DataMapperResolverTasks(contextName);
            var dataMappers = DataMappers(contextName);
            container.Register(typeTasks.ToArray());
            container.Register(configTasks.ToArray());
            container.Register(objectTasks.ToArray());
            container.Register(dataMapperTasks.ToArray());
            container.Register(dataMappers.ToArray());
        }

        public abstract IEnumerable<ComponentRegistration<AbstractDataMapper>> DataMappers(string contextName);
        public abstract IEnumerable<ComponentRegistration<IDataMapperResolverTask>> DataMapperResolverTasks(string contextName);
        public abstract IEnumerable<ComponentRegistration<IObjectConstructionTask>> ObjectContructionTasks(string contextName);
        public abstract IEnumerable<ComponentRegistration<ITypeResolverTask>> TypeResolverTasks(string contextName);
        public abstract IEnumerable<ComponentRegistration<IConfigurationResolverTask>> ConfigurationResolverTasks(string contextName);
       
    }
}
