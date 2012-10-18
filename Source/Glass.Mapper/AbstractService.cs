using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.TypeResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver;

namespace Glass.Mapper
{
    public abstract class  AbstractService <T> where T : IDataContext
    {
        private Context _context;

        public AbstractService()
            :this(Context.Default)
        {
        }

        public AbstractService(string contextName)
            : this(Context.Contexts[contextName])
        {
        }

        public AbstractService(Context context)
        {
            _context = context;
            if (_context == null) throw new NullReferenceException("Context is null");
        }

        public static object InstantiateObject(Context context, T dataContext)
        {
            //Run the get type pipeline to get the type to load
            var typeRunner = new TypeResolverRunner(context.TypeResolverTasks);
            var typeArgs = new TypeResolverArgs(context, dataContext);
            typeRunner.Run(typeArgs);

            //TODO: ME - make these exceptions more specific
            if(typeArgs.Result == null)
                throw new NullReferenceException("Type Resolver pipeline did not return type.");

            //run the pipeline to get the configuration to load
            var configurationRunner = new ConfigurationResolverRunner(context.ConfigurationResolverTasks);
            var configurationArgs = new ConfigurationResolverArgs(context, dataContext, typeArgs.Result);
            configurationRunner.Run(configurationArgs);

            if (configurationArgs.Result == null)
                throw new NullReferenceException("Configuration Resolver pipeline did not return type.");

            //Run the object construction
            var objectRunner = new ObjectConstructionRunner(context.ObjectConstructionTasks);
            var objectArgs = new ObjectConstructionArgs(context, dataContext, configurationArgs.Result);
            objectRunner.Run(objectArgs);

            return objectArgs.Result;
        }
    }
}
