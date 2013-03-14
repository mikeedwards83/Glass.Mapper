using System;
using System.Collections.Generic;
using Sitecore.Web.UI;

namespace Glass.Mapper.Sc.Razor.RenderingTypes
{
    /// <summary>
    /// Class AbstractCachingRenderingType
    /// </summary>
    public abstract class AbstractCachingRenderingType : RenderingType
    {
        private static readonly object _key = new object();
        private static readonly object _typeKey = new object();

        /// <summary>
        /// Gets the loaded types.
        /// </summary>
        /// <value>The loaded types.</value>
        protected  static  Dictionary<string, Type> LoadedTypes { get; private set; }

        /// <summary>
        /// Initializes static members of the <see cref="AbstractCachingRenderingType"/> class.
        /// </summary>
        static AbstractCachingRenderingType()
        {
            if (LoadedTypes == null)
            {
                lock (_key)
                {
                    if (LoadedTypes == null)
                    {
                        LoadedTypes = new Dictionary<string, Type>();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the type of the control.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="typeLoader">The type loader.</param>
        /// <returns>Type.</returns>
        /// <exception cref="System.NullReferenceException">Could not find type {0} for Razor view..Formatted(typeName)</exception>
        public static Type GetControlType(string typeName, Func<string, Type> typeLoader)
        {
            Type finalType = null;

            if (LoadedTypes.ContainsKey(typeName))
                finalType = LoadedTypes[typeName];
            else
            {
                finalType = typeLoader(typeName);
                if (finalType == null) throw new NullReferenceException("Could not find type {0} for Razor view.".Formatted(typeName));

                //we added to the collection making sure no one else added it before
                if (!LoadedTypes.ContainsKey(typeName))
                {
                    lock (_typeKey)
                    {
                        if (!LoadedTypes.ContainsKey(typeName))
                        {
                            LoadedTypes.Add(typeName, finalType);
                        }
                    }
                }
            }

            return finalType;
        }
    }
}
