using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper.Maps
{
    public class GlassMapLoader
    {
        private readonly Assembly _assembly;

        /// <summary>
        /// Creates an instance of  the GlassMapLoader
        /// </summary>
        /// <param name="assembly">The assembly to load IGlassMaps from</param>
        public GlassMapLoader(Assembly assembly)
        {
            _assembly = assembly;
        }

        /// <summary>
        /// Creates an instance of  the GlassMapLoader
        /// </summary>
        /// <param name="type">A type in the assembly to load IGlassMaps from</param>
        public GlassMapLoader(Type type)
        {
            _assembly = type.Assembly;
        }


        public IEnumerable<IGlassMap> Load()
        {
            Type glassmapType = typeof(IGlassMap);
            IEnumerable<Type> mapTypes = _assembly.GetTypes().Where(x => glassmapType.IsAssignableFrom(x));
            var maps = new List<IGlassMap>();

            foreach (var mapType in mapTypes)
            {
                try
                {
                    var activator = ActivationManager.GetActivator<IGlassMap>(mapType);
                    var map = activator();
                    maps.Add(map);
                }
                catch (Exception ex)
                {
                    throw new MapperException("Failed to create instance of IGlassMap.", ex);
                }
            }

            return maps;
        }

       
    }
}
