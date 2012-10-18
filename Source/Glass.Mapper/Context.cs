using System.Collections.Generic;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface;
using Glass.Mapper.Pipelines.TypeResolver;
using Glass.Mapper.Pipelines.TypeResolver.Tasks.StandardResolver;

namespace Glass.Mapper
{
    /// <summary>
    /// The context contains the configuration of Glass.Mapper
    /// </summary>
    public class Context
    {

        #region STATICS

        static Context()
        {
            Contexts = new Dictionary<string, Context>();
        }

        public static Context Default { get; private set; }
        public static IDictionary<string, Context> Contexts { get; private set; }


        /// <summary>
        /// Loads a context and creates it as the default Context
        /// </summary>
        public static Context Load(params IConfigurationLoader [] loaders)
        {
            return Context.Load("default", true, loaders);
        }
        public static Context Load(string contextName, bool isDefault = false, params IConfigurationLoader[] loaders)
        {
            var context = new Context();
            context.LoadContext(loaders);

            Contexts[contextName] = context;
            
            if (isDefault)
                Default = context;

            return context;
        }

        #endregion

        public IEnumerable<AbstractTypeConfiguration> TypeConfigurations { get; private set; }
        public IList<IObjectConstructionTask> ObjectConstructionTasks { get; private set; }
        public IList<ITypeResolverTask> TypeResolverTasks { get; private set; }
        public IList<IConfigurationResolverTask> ConfigurationResolverTasks { get; private set; }

       
        private Context()
        {
            ObjectConstructionTasks = new List<IObjectConstructionTask>();
            TypeResolverTasks = new List<ITypeResolverTask>();
            ConfigurationResolverTasks = new List<IConfigurationResolverTask>();

            //TODO: ME - not sure if this is the best place for this
            ObjectConstructionTasks.Add(new CreateConcreteTask());
            ObjectConstructionTasks.Add(new CreateInterfaceTask());

            TypeResolverTasks.Add(new TypeStandardResolverTask());

        }

        private void LoadContext(IConfigurationLoader [] loaders)
        {
            var typeConfigs = new List<AbstractTypeConfiguration>();
            loaders.ForEach(x => typeConfigs.AddRange(x.Load()));
            TypeConfigurations = typeConfigs;
        }



    }


    
}
