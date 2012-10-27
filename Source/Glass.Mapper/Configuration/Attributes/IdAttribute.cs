using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Configuration.Attributes
{
    public class IdAttribute : AbstractPropertyAttribute
    {
        public IdAttribute(Type idType)
        {
            IdType = idType;
        }

        public Type IdType { get; set; }

        public override AbstractPropertyConfiguration Configure(System.Reflection.PropertyInfo propertyInfo)
        {
            throw new NotImplementedException();
        }
    }
}
