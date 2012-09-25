using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper
{
    public class MapperException : Exception 
    {
        public MapperException(string message):base(message)
        {
        }
    }
}
