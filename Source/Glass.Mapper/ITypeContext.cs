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
    public interface ITypeContext
    {
        bool InferType { get; }
        bool IsLazy { get; set; }
        Type RequestedType { get; }
        object[] ConstructorParameters { get; }
    }
}