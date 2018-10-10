using System;

namespace Glass.Mapper
{
    public class ConstructorParameter
    {
        public Type Type { get; set; }
        public object Value { get; set; }

        public ConstructorParameter(Type type, object value)
        {
            Type = type;
            Value = value;
        }
        public ConstructorParameter( object value) 
            : this(value.GetType(), value)
        {
        }
    }

    public class ConstructorParameter<T> : ConstructorParameter
    {
        public ConstructorParameter(object value) : base(typeof(T), value)
        {

        }

    }
}
