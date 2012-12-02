using System.Collections;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.CastleWindsor
{
    public class CastleDependencyResolver : IDependencyResolver
    {
        private WindsorContainer _container;

        public T Resolve<T>(IDictionary<string, object> args = null)
        {
            if (args == null)
                return _container.Resolve<T>();
            else
                return _container.Resolve<T>((IDictionary)args);
        }

        public void Load(string contextName, IGlassConfiguration config)
        {

            var castleConfig = config as GlassCastleConfigBase;
            if(castleConfig == null)
                throw new MapperException("IGlassConfiguration is not of type GlassCastleConfigBase");

            _container = new WindsorContainer();
            castleConfig.Setup(_container, contextName);

        }


        public IEnumerable<T> ResolveAll<T>()
        {
            return _container.ResolveAll<T>();
        }
    }
}
