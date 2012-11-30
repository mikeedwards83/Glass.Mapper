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

        public ObjectFactory _factory;
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

            var args = new Dictionary<string, object>() {{"context", _context}};
            _factory = context.DependencyResolver.Resolve<ObjectFactory>(args);
        }

        
    }
}
