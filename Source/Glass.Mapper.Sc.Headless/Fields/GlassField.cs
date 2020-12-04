using System;
using System.Reflection;

namespace Glass.Mapper.Sc.Fields
{
    /// <summary>
    /// Represents a Field in Glass
    /// </summary>
    public class GlassField
    {
        public Type Type { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}