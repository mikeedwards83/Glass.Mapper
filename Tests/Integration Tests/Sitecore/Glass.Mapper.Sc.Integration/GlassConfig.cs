using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.StandardResolver;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Pipelines.DataMapperResolver.Tasks;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface;
using Glass.Mapper.Pipelines.TypeResolver;
using Glass.Mapper.Pipelines.TypeResolver.Tasks.StandardResolver;
using Glass.Mapper.Sc.DataMappers;

namespace Glass.Mapper.Sc.Integration
{
    public class GlassConfig : GlassCastleConfigBase
    {

        public override IEnumerable<ComponentRegistration<AbstractDataMapper>> DataMappers(string contextName)
        {
            return new[]
                       {
                           Component.For<AbstractDataMapper>().ImplementedBy<SitecoreInfoMapper>().LifestyleTransient(),
                           Component.For<AbstractDataMapper>().ImplementedBy<SitecoreIdMapper>().LifestyleTransient()
                       };

        }

        public override IEnumerable<ComponentRegistration<IDataMapperResolverTask>> DataMapperResolverTasks(string contextName)
        {
            //****** Data Mapper Resolver Tasks ******//
            // These tasks are run when Glass.Mapper tries to resolve which DataMapper should handle a given property, e.g. 
            // Tasks are called in the order they are specified below.
            // For more on component registration read: http://docs.castleproject.org/Windsor.Registering-components-one-by-one.ashx

            return new[]
                    {
                        Component.For<IDataMapperResolverTask>().ImplementedBy<DataMapperStandardResolverTask>().LifestyleTransient(),
                    };
        }
        public override IEnumerable<ComponentRegistration<ITypeResolverTask>> TypeResolverTasks(string contextName)
        {
            //****** Type Resolver Tasks ******//
            // These tasks are run when Glass.Mapper tries to resolve the type a user has requested, e.g. 
            // if your code contained
            //       service.GetItem<MyClass>(id) 
            // the standard resolver will return MyClass as the type. You may want to specify your own tasks to custom type
            // inferring.
            // Tasks are called in the order they are specified below.
            // For more on component registration read: http://docs.castleproject.org/Windsor.Registering-components-one-by-one.ashx

            return new[]
                    {
                        Component.For<ITypeResolverTask>().ImplementedBy<TypeStandardResolverTask>().LifestyleTransient(),
                    };
        }
        public override IEnumerable<ComponentRegistration<IConfigurationResolverTask>> ConfigurationResolverTasks(string contextName)
        {
            //****** Configuration Resolver Tasks ******//
            // These tasks are run when Glass.Mapper tries to find the configration the user has requested based on the type passsed, e.g. 
            // if your code contained
            //       service.GetItem<MyClass>(id) 
            // the standard resolver will return the MyClass configuration. 
            // Tasks are called in the order they are specified below.
            // For more on component registration read: http://docs.castleproject.org/Windsor.Registering-components-one-by-one.ashx

            return new[]
                    {
                        Component.For<IConfigurationResolverTask>().ImplementedBy<ConfigurationStandardResolverTask>().LifestyleTransient(),
                    };
        }
        public override IEnumerable<ComponentRegistration<IObjectConstructionTask>> ObjectContructionTasks(string contextName)
        {
            //****** Object Construction Tasks ******//
            // These tasks are run when an a class needs to be instantiated by Glass.Mapper.
            // Tasks are called in the order they are specified below.
            // For more on component registration read: http://docs.castleproject.org/Windsor.Registering-components-one-by-one.ashx

            return
                new[]
                    {
                        Component.For<IObjectConstructionTask>().ImplementedBy<CreateConcreteTask>().LifestyleTransient(),
                        Component.For<IObjectConstructionTask>().ImplementedBy<CreateInterfaceTask>().LifestyleTransient(),
                    };
        }


    }
}
