using System;
using System.Collections.Generic;
using Sitecore.Web.UI;

namespace Glass.Mapper.Sc.Razor.RenderingTypes
{
    public abstract class AbstractCachingRenderingType : RenderingType
    {

        private static readonly object _key = new object();
        private static readonly object _typeKey = new object();

        protected  static  Dictionary<string, Type> LoadedTypes { get; private set; }

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
