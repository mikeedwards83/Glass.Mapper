using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper
{
    public class MapperStackException : MapperException

    {
        public MapperStackException(string message) : base(message)
        {
        }

        public MapperStackException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}
