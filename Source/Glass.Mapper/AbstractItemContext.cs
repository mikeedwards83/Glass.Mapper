using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Glass.Mapper
{
    /// <summary>
    /// The base class for the context loading an item from the CMS
    /// </summary>
    public abstract class AbstractItemContext
    {
        public AbstractItemContext()
        {
            ConstructorMethods = new Dictionary<ConstructorInfo, Delegate>();
        }


        public Type Type { get; set; }

        public Dictionary<ConstructorInfo, Delegate> ConstructorMethods { get; private set; }

        public IEnumerable<AbstractDataMapper> DataMappers { get; set; }

    }
}