using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Glass.Mapper.Configuration;

namespace Glass.Mapper
{
    /// <summary>
    /// The base class for the context loading an item from the CMS
    /// </summary>
    public abstract class AbstractTypeCreationContext
    {
      

        public bool InferType { get; set; }
        public bool IsLazy { get; set; }
        public Type RequestedType { get; set; }
        public object[] ConstructorParameters { get; set; }
    }
}